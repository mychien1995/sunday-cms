import { Component, OnInit } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { Router, ActivatedRoute } from '@angular/router';
import { LayoutItem, OrganizationItem } from '@models/index';
import { LayoutManagementService, OrganizationService } from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subscription, forkJoin } from 'rxjs';

@Component({
  selector: 'app-add-layout',
  templateUrl: './add-layout.component.html',
})
export class AddLayoutComponent implements OnInit {
  public dataForm: FormGroup = new FormGroup({
    LayoutName: new FormControl('', [Validators.required]),
    LayoutPath: new FormControl('', [Validators.required]),
  });
  public organizationLookup: OrganizationItem[] = [];
  public isEdit: boolean;
  public current: LayoutItem = new LayoutItem();

  public formTitle = 'Create Layout';
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private clientState: ClientState,
    private organizationService: OrganizationService,
    private layoutService: LayoutManagementService,
    private toastr: ToastrService
  ) {
    this.activatedRoute.data.subscribe((data: { layout: LayoutItem }) => {
      if (data.layout) {
        this.isEdit = true;
        this.current = data.layout;
        this.formTitle = 'Edit Layout';
      }
    });
  }

  ngOnInit(): void {
    this.getOrganizations();
    this.buildForm();
  }

  buildForm(): void {
    if (this.isEdit) {
      this.dataForm = new FormGroup({
        LayoutName: new FormControl(this.current.LayoutName, [
          Validators.required,
        ]),
        LayoutPath: new FormControl(this.current.LayoutPath, [
          Validators.required,
        ]),
      });
    }
  }

  getOrganizations(): void {
    this.organizationService.getOrganizationsLookup().subscribe((res) => {
      if (res.Success) {
        this.organizationLookup = res.List;
      }
    });
  }

  onSubmit(formValue: any): void {
    if (!this.dataForm.valid) {
      return;
    }
    const data = <LayoutItem>formValue;
    data.OrganizationIds = this.current.OrganizationIds;
    data.Id = this.isEdit ? this.current.Id : '';
    this.clientState.isBusy = true;
    const observ = this.isEdit
      ? this.layoutService.updateLayout(data)
      : this.layoutService.createLayouts(data);
    observ.subscribe(
      (res) => {
        if (res.Success) {
          this.toastr.success(
            this.isEdit ? 'Layout updated' : 'Layout created'
          );
          this.router.navigate(['/manage-layouts']);
        }
        this.clientState.isBusy = false;
      },
      (ex) => (this.clientState.isBusy = false)
    );
  }
}
