import { Component, OnInit } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Rendering } from '@models/index';
import { RenderingService } from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-rendering',
  templateUrl: './add-rendering.component.html',
})
export class AddRenderingComponent implements OnInit {
  public dataForm: FormGroup = new FormGroup({
    RenderingName: new FormControl('', [Validators.required]),
    Controller: new FormControl('', [Validators.required]),
    Action: new FormControl('', [Validators.required]),
    IsPageRendering: new FormControl(false),
    IsRequireDatasource: new FormControl(false),
    DatasourceTemplate: new FormControl(''),
    DatasourceLocation: new FormControl(''),
  });
  public isEdit: boolean;
  public current: Rendering = new Rendering();

  public formTitle = 'Create Rendering';
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private clientState: ClientState,
    private renderingService: RenderingService,
    private toastr: ToastrService
  ) {
    this.activatedRoute.data.subscribe((data: { rendering: Rendering }) => {
      if (data.rendering) {
        this.isEdit = true;
        this.current = data.rendering;
        this.formTitle = 'Edit Rendering';
      }
    });
  }

  ngOnInit(): void {
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
