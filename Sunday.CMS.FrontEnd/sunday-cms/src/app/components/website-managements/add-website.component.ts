import { Component, OnInit } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { Router, ActivatedRoute } from '@angular/router';
import { WebsiteItem, OrganizationItem, LayoutItem } from '@models/index';
import { WebsiteManagementService, OrganizationService, LayoutService, LayoutManagementService } from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subscription, forkJoin } from 'rxjs';

@Component({
  selector: 'app-add-website',
  templateUrl: './add-Website.component.html',
})
export class AddWebsiteComponent implements OnInit {
  public dataForm: FormGroup = new FormGroup({
    WebsiteName: new FormControl('', [Validators.required]),
    IsActive: new FormControl(true),
    LayoutId : new FormControl('', [Validators.required]),
  });
  public layoutLookup: LayoutItem[] = [];
  public isEdit: boolean;
  public current: WebsiteItem = new WebsiteItem();

  public formTitle = 'Create Website';
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private clientState: ClientState,
    private layoutService: LayoutManagementService,
    private WebsiteService: WebsiteManagementService,
    private toastr: ToastrService
  ) {
    this.activatedRoute.data.subscribe((data: { Website: WebsiteItem }) => {
      if (data.Website) {
        this.isEdit = true;
        this.current = data.Website;
        this.formTitle = 'Edit Website';
      }
    });
  }

  ngOnInit(): void {
    this.getLayouts();
    this.buildForm();
  }

  buildForm(): void {
    if (this.isEdit) {
      this.dataForm = new FormGroup({
        WebsiteName: new FormControl(this.current.WebsiteName, [
          Validators.required,
        ]),
        IsActive: new FormControl(true),
        LayoutId : new FormControl(this.current.LayoutId, [Validators.required]),
      });
    }
  }

  getLayouts(): void {
    this.layoutService.getLayouts({PageSize : 1000}).subscribe((res) => {
      if (res.Success) {
        this.layoutLookup = res.Layouts;
      }
    });
  }

  onSubmit(formValue: any): void {
    if (!this.dataForm.valid) {
      return;
    }
    const data = <WebsiteItem>formValue;
    data.HostNames = this.current.HostNames;
    data.Id = this.isEdit ? this.current.Id : '';
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
      (ex) => (this.clientState.isBusy = false)
    );
  }
}
