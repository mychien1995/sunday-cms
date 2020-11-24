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
        this.dataForm.controls['IsPageRendering'].setValue(
          this.current.IsPageRendering
        );
        this.dataForm.controls['IsRequireDatasource'].setValue(
          this.current.IsRequireDatasource
        );
      }
    });
  }

  ngOnInit(): void {}

  onSubmit(formValue: any): void {
    if (!this.dataForm.valid) {
      return;
    }
    const data = { ...this.current, ...(<Rendering>formValue) };
    data.Id = this.isEdit ? this.current.Id : '';
    data.Access = this.current.Access;
    this.clientState.isBusy = true;
    const observ = this.isEdit
      ? this.renderingService.update(data)
      : this.renderingService.create(data);
    observ.subscribe(
      (res) => {
        if (res.Success) {
          this.toastr.success(
            this.isEdit ? 'Rendering updated' : 'Rendering created'
          );
          this.router.navigate(['/manage-renderings']);
        }
        this.clientState.isBusy = false;
      },
      (ex) => (this.clientState.isBusy = false)
    );
  }
}
