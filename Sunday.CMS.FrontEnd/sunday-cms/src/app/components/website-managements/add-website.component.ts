import { Component, OnInit } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { Router, ActivatedRoute } from '@angular/router';
import {
  WebsiteItem,
  LayoutItem,
  TemplateItem,
  Rendering,
  OrganizationItem,
} from '@models/index';
import {
  WebsiteManagementService,
  LayoutManagementService,
  TemplateManagementService,
  RenderingService,
  AuthenticationService,
  OrganizationService,
} from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-add-website',
  templateUrl: './add-Website.component.html',
})
export class AddWebsiteComponent implements OnInit {
  public dataForm: FormGroup = new FormGroup({
    WebsiteName: new FormControl('', [Validators.required]),
    IsActive: new FormControl(true),
    LayoutId: new FormControl('', [Validators.required]),
    OrganizationId: new FormControl('', [Validators.required]),
  });
  public layoutLookup: LayoutItem[] = [];
  public isEdit: boolean;
  public current: WebsiteItem = new WebsiteItem();
  public templateLookup: TemplateItem[] = [];
  public renderingLookup: Rendering[] = [];
  organizationLookup: OrganizationItem[] = [];

  public formTitle = 'Create Website';
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private clientState: ClientState,
    private layoutService: LayoutManagementService,
    private WebsiteService: WebsiteManagementService,
    private templateService: TemplateManagementService,
    private organizationSerivce: OrganizationService,
    private renderingService: RenderingService,
    private authService: AuthenticationService,
    private toastr: ToastrService
  ) {
    this.activatedRoute.data.subscribe((data: { website: WebsiteItem }) => {
      if (data.website) {
        this.isEdit = true;
        this.current = data.website;
        this.formTitle = 'Edit Website';
      }
      this.getTemplatesAndRenderings();
    });
  }

  ngOnInit(): void {
    this.buildForm();
    this.getLayouts();
    this.getOrganizations();
  }

  buildForm(): void {
    if (this.isEdit) {
      this.dataForm = new FormGroup({
        WebsiteName: new FormControl(this.current.WebsiteName, [
          Validators.required,
        ]),
        IsActive: new FormControl(true),
        LayoutId: new FormControl(this.current.LayoutId, [Validators.required]),
        OrganizationId: new FormControl(
          this.current.OrganizationId,
          !this.isSysAdmin() ? [Validators.required] : []
        ),
      });
    }
  }

  getTemplatesAndRenderings(orgId?: string): void {
    const query = {
      PageSize: 100000,
      WebsiteId: this.current.Id,
      IsAbstract: false,
      IsPageRendering: true,
      IsPageTemplate: true,
      OrganizationId: orgId,
    };
    this.clientState.isBusy = true;
    forkJoin([
      this.templateService.getTemplates(query),
      this.renderingService.getRenderings(query),
    ]).subscribe(
      (res) => {
        this.templateLookup = res[0].Templates;
        this.renderingLookup = res[1].List;
        this.clientState.isBusy = false;
      },
      (ex) => (this.clientState.isBusy = false)
    );
  }
  getLayouts(orgId?: string): void {
    this.layoutService
      .getLayouts({ PageSize: 100000, OrganizationId: orgId })
      .subscribe((res) => {
        if (res.Success) {
          this.layoutLookup = res.Layouts;
        }
      });
  }

  getOrganizations() {
    if (this.isSysAdmin()) {
      this.organizationSerivce
        .getOrganizations({ PageSize: 10000 })
        .subscribe((res) => {
          this.organizationLookup = res.Organizations;
        });
    }
  }

  organizationChanged(data): void {
    const orgId = data.Id;
    this.getLayouts(orgId);
    this.getTemplatesAndRenderings(orgId);
  }

  onSubmit(formValue: any): void {
    if (!this.dataForm.valid) {
      return;
    }
    const data = <WebsiteItem>formValue;
    data.HostNames = this.current.HostNames;
    data.Id = this.isEdit ? this.current.Id : '';
    data.PageDesignMappings = this.current.PageDesignMappings;
    this.clientState.isBusy = true;
    const observ = this.isEdit
      ? this.WebsiteService.updateWebsite(data)
      : this.WebsiteService.createWebsite(data);
    observ.subscribe(
      (res) => {
        if (res.Success) {
          this.toastr.success(
            this.isEdit ? 'Website updated' : 'Website created'
          );
          this.router.navigate(['/manage-websites']);
        }
        this.clientState.isBusy = false;
      },
      () => (this.clientState.isBusy = false)
    );
  }

  isSysAdmin = () => this.authService.isSysAdmin();
}
