import {
  Component,
  OnInit,
  ViewEncapsulation,
  TemplateRef,
  ChangeDetectorRef,
} from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import {
  OrganizationService,
  ColorService,
  FileUploadService,
  ModuleService,
} from '@services/index';
import {
  OrganizationMutationModel,
  OrganizationDetailResponse,
  ModuleModel,
} from '@models/index';
import { DefaultOrganizationLogo } from '@core/constants';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-organization-form',
  templateUrl: './organization-form.component.html',
})
export class OrganizationFormComponent implements OnInit {
  public isEdit: boolean;
  public colorList: any[] = [];
  public hostNames: string[] = [''];
  public logoImage: string = DefaultOrganizationLogo;
  public logoImageFile: File;
  public currentOrganization: OrganizationDetailResponse;
  public modules: ModuleModel[];
  public selectedModules: ModuleModel[] = [];

  public organizationForm: FormGroup = new FormGroup({
    Name: new FormControl('', [Validators.required]),
    Description: new FormControl(''),
    IsActive: new FormControl(true, [Validators.required]),
    Color: new FormControl(true, [Validators.required]),
    Logo: new FormControl('', [Validators.required]),
  });

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private colorService: ColorService,
    private fileUploadService: FileUploadService,
    private organizationService: OrganizationService,
    private toastr: ToastrService,
    private clientState: ClientState,
    private moduleService: ModuleService
  ) {
    this.activatedRoute.data.subscribe(
      (data: { organization: OrganizationDetailResponse }) => {
        if (data.organization) {
          this.isEdit = true;
          this.currentOrganization = data.organization;
          this.logoImage =
            data.organization.LogoLink || DefaultOrganizationLogo;
          this.hostNames = data.organization.HostNames;
          this.hostNames.push('');
          this.selectedModules = data.organization.ModuleIds.map(
            (c) =>
              <ModuleModel>{
                Id: c,
              }
          );
        }
      }
    );
  }

  ngOnInit(): void {
    this.getColors();
    this.getModules();
    if (this.isEdit) {
      this.organizationForm = new FormGroup({
        Name: new FormControl(this.currentOrganization.OrganizationName, [
          Validators.required,
        ]),
        Description: new FormControl(this.currentOrganization.Description),
        IsActive: new FormControl(this.currentOrganization.IsActive, [
          Validators.required,
        ]),
        Color: new FormControl(this.currentOrganization.ColorScheme, [
          Validators.required,
        ]),
        Logo: new FormControl('', !this.isEdit ? [Validators.required] : []),
      });
    }
  }

  onSubmit(formValue: any): void {
    if (
      this.organizationForm.invalid ||
      !this.hostNames.filter((x) => x && x.trim().length > 0)
    ) {
      return;
    }
    this.clientState.isBusy = true;
    let promise = new Observable<any>();
    if (this.logoImageFile) {
      promise = this.fileUploadService.uploadBlob('logos', this.logoImageFile);
    } else {
      promise = new Observable((obs) => {
        obs.next({
          BlobIdentifier: this.currentOrganization?.LogoBlobUri,
          Success: true,
        });
        obs.complete();
      });
    }
    promise.subscribe((res) => {
      if (res.Success) {
        const blobIdentifier = res.BlobIdentifier;
        const mutationData = <OrganizationMutationModel>{
          ColorScheme: formValue.Color,
          Description: formValue.Description,
          OrganizationName: formValue.Name,
          IsActive: formValue.IsActive,
          HostNames: this.hostNames,
          LogoBlobUri: blobIdentifier,
          Id: this.currentOrganization?.Id || '',
          ModuleIds: this.selectedModules.map((c) => c.Id),
        };
        (this.isEdit
          ? this.organizationService.updateOrganization(mutationData)
          : this.organizationService.createOrganization(mutationData)
        ).subscribe((response) => {
          if (response.Success) {
            this.toastr.success(
              this.isEdit ? 'Organization Updated' : 'Organization Created'
            );
            this.router.navigate(['/organizations']);
          }
          this.clientState.isBusy = false;
        }, err => this.clientState.isBusy = false);
      }
    });
  }

  getColors(): void {
    this.colorList = this.colorService.getColors();
  }

  getModules(): void {
    this.moduleService.getModules().subscribe((res) => {
      if (res.Success) {
        this.modules = res.Modules;
        const moduleIds = this.selectedModules.map((c) => c.Id);
        this.selectedModules = this.modules.filter(
          (c) => moduleIds.indexOf(c.Id) > -1
        );
      }
    });
  }

  addHostNames(index: number): void {
    if (
      this.hostNames[index].trim().length === 0 &&
      index !== this.hostNames.length - 1
    ) {
      this.hostNames.splice(index, 1);
      return;
    }
    if (this.hostNames.filter((x) => !x || x.trim().length === 0).length > 0) {
      return;
    }
    this.hostNames.push('');
  }

  onSelectAvatar(event: any): void {
    const file: File = event.target.files && <File>event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (ev: any) => {
        this.logoImage = ev.target.result;
      };
      this.logoImageFile = file;
    }
  }

  trackByFn(index: any, item: any) {
    return index;
  }
}
