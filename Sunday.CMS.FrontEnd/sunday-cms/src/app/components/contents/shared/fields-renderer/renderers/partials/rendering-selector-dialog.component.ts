import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Rendering, RenderingValue } from '@models/index';
import { RenderingService } from '@services/index';
import { DatasourceSelectorDialogComponent } from './datasource-selector.component';

@Component({
  selector: 'app-rendering-selector',
  templateUrl: './rendering-selector-dialog.component.html',
})
export class RenderingSelectorDialogComponent implements OnInit {
  renderingLookup: Rendering[] = [];
  websiteId: string;
  callback: (rendering: RenderingValue) => any;
  constructor(
    private renderingService: RenderingService,
    private dialogService: MatDialog
  ) {}

  selectedRendering: Rendering;

  load(
    callback: (rendering: RenderingValue) => any,
    websiteId: string,
    contentId?: string
  ) {
    this.callback = callback;
    this.websiteId = websiteId;
    this.renderingService
      .getRenderings({
        IsPageRendering: false,
        WebsiteId: websiteId,
        PageSize: 1000,
      })
      .subscribe((res) => {
        this.renderingLookup = res.List;
      });
  }

  onSelect(rendering: Rendering): void {
    this.selectedRendering = rendering;
  }
  onSubmit(): void {
    if (this.selectedRendering) {
      if (this.selectedRendering.IsRequireDatasource) {
        const ref = this.dialogService.open(DatasourceSelectorDialogComponent, {
          minWidth: 600,
        });
        ref.componentInstance.load(
          this.selectedRendering,
          this.websiteId,
          (datasourceId) => {
            this.callback(<RenderingValue>{
              RenderingId: this.selectedRendering.Id,
              Datasource: datasourceId,
            });
            this.dialogService.closeAll();
          }
        );
      } else {
        this.callback(<RenderingValue>{
          RenderingId: this.selectedRendering.Id,
        });
        this.dialogService.closeAll();
      }
    }
  }
  ngOnInit(): void {}
}
