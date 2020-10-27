import {
  Component,
  OnInit,
  ViewEncapsulation,
  TemplateRef,
} from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { Router, ActivatedRoute } from '@angular/router';
import {
  OrganizationRoleDetailApiResponse,
  FeatureItem,
  OrganizationRoleMutationData,
  ModuleListApiResponse,
  ModuleModel,
} from '@models/index';
import {
  FeatureService,
  OrganizationRoleService,
  OrganizationService,
} from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subscription, forkJoin } from 'rxjs';

@Component({
  selector: 'app-add-organization-role',
  templateUrl: './add-organization-role.component.html',
})
export class AddOrganizationRoleComponent implements OnInit {
  public roleForm: FormGroup = new FormGroup({
    RoleName: new FormControl('', [Validators.required]),
  });
  public moduleLookup: ModuleModel[] = [];
  public isEdit: boolean;
  public currentRole: OrganizationRoleDetailApiResponse = new OrganizationRoleDetailApiResponse();

  public formTitle = 'Create Role';
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private featureService: FeatureService,
    private clientState: ClientState,
    private roleService: OrganizationRoleService,
    private organizationService: OrganizationService,
    private toastr: ToastrService
  ) {
    this.activatedRoute.data.subscribe(
      (data: { role: OrganizationRoleDetailApiResponse }) => {
        if (data.role) {
          this.isEdit = true;
          this.currentRole = data.role;
          this.formTitle = 'Edit Role';
        }
      }
    );
  }
  ngOnInit(): void {
    this.getFeatures();
    this.buildForm();
  }

  buildForm(): void {
    if (this.isEdit) {
      this.roleForm = new FormGroup({
        RoleName: new FormControl(this.currentRole.RoleName, [
          Validators.required,
        ]),
      });
    }
  }

  getFeatures(): void {
    this.organizationService.getModules().subscribe((res) => {
      if (res.Success) {
        this.moduleLookup = res.Modules;
      }
    });
  }

  onSubmit(formValue: any): void {
    if (!this.roleForm.valid || this.currentRole.FeatureIds.length === 0) {
      return;
    }
    const data = <OrganizationRoleMutationData>formValue;
    data.FeatureIds = this.currentRole.FeatureIds;
    data.Id = this.isEdit ? this.currentRole.Id : '';
    this.clientState.isBusy = true;
    const observ = this.isEdit
      ? this.roleService.updateRole(data)
      : this.roleService.createRole(data);
    observ.subscribe((res) => {
      if (res.Success) {
        this.toastr.success(this.isEdit ? 'Role updated' : 'Role created');
        this.router.navigate(['/organization-roles']);
      }
      this.clientState.isBusy = false;
    }, ex => this.clientState.isBusy = false);
  }
}
