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
    Controller: new FormControl(''),
    Action: new FormControl(''),
    Component: new FormControl(''),
    IsRequireDatasource: new FormControl(false),
    DatasourceLocation: new FormControl(''),
    RenderingType: new FormControl('', [Validators.required])
  });
  public isEdit: boolean;
  public current: Rendering = new Rendering();
  selectedTemplates: string[] = [];
  renderingTypes: any[] = [];

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
        this.dataForm.controls['IsRequireDatasource'].setValue(
          this.current.IsRequireDatasource
        );
        this.dataForm.controls['RenderingType'].setValue(
          this.current.RenderingType
        );
        this.selectedTemplates =
          this.current?.DatasourceTemplate &&
          this.current.DatasourceTemplate.trim().length > 0
            ? [this.current.DatasourceTemplate]
            : [];
      }
    });
  }

  onPageRenderingChanged(): void {
    if (this.IsPageRendering) {
      this.dataForm.controls['IsRequireDatasource'].setValue(false);
      this.current.IsRequireDatasource = false;
      this.current.DatasourceLocation = '';
      this.current.DatasourceTemplate = '';
      this.selectedTemplates = [];
    }
  }

  get IsPageRendering(): boolean {
    return this.current?.RenderingType === 'PageRendering';
  }

  ngOnInit(): void {
    this.renderingService.getRenderingTypes().subscribe((res) => {
      this.renderingTypes = res.data;
    });
  }

  onSubmit(formValue: any): void {
    if (!this.dataForm.valid) {
      return;
    }
    this.current.DatasourceTemplate =
      this.selectedTemplates.find((t) => t && t.trim().length > 0) ?? null;
    const data = {
      ...this.current,
      ...(<Rendering>formValue),
      ...{ IsPageRendering: false },
    };
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
