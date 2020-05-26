import {
  Component,
  OnInit,
  ViewEncapsulation,
  TemplateRef,
} from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import {
  OrganizationService,
  ColorService,
  FileUploadService,
} from '@services/index';
import { OrganizationMutationModel } from '@models/index';
import { DefaultOrganizationLogo } from '@core/constants';
import { ToastrService } from 'ngx-toastr';

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
  public organizationForm: FormGroup = new FormGroup({
    Name: new FormControl('', [Validators.required]),
    Description: new FormControl('', [Validators.required]),
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
    private clientState: ClientState
  ) {
    this.activatedRoute.paramMap.subscribe((param) => {
      const orgId = param.get('orgId');
      if (orgId) {
        this.isEdit = true;
      }
    });
  }

  ngOnInit(): void {
    this.getColors();
  }

  onSubmit(formValue: any): void {
    if (
      this.organizationForm.invalid &&
      !this.hostNames.filter((x) => x && x.trim().length > 0)
    ) {
      return;
    }
    this.clientState.isBusy = true;
    this.fileUploadService
      .uploadBlob('logos', this.logoImageFile)
      .subscribe((res) => {
        if (res.Success) {
          const blobIdentifier = res.BlobIdentifier;
          const mutationData = <OrganizationMutationModel>{
            ColorScheme: formValue.Color,
            Description: formValue.Description,
            OrganizationName: formValue.Name,
            IsActive: formValue.IsActive,
            HostNames: this.hostNames,
            LogoBlobUri: blobIdentifier,
          };
          this.organizationService
            .createOrganization(mutationData)
            .subscribe((createResponse) => {
              if (createResponse.Success) {
                this.toastr.success('Organization Created');
                this.router.navigate(['/organizations']);
              }
              this.clientState.isBusy = false;
            });
        }
      });
  }

  getColors(): void {
    this.colorList = this.colorService.getColors();
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
