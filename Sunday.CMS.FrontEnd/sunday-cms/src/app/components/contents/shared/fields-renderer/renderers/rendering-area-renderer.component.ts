import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  ContentField,
  Rendering,
  RenderingValue,
  TemplateField,
} from '@models/index';
import { RenderingSelectorDialogComponent } from './partials/rendering-selector-dialog.component';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { RenderingService } from '@services/index';

@Component({
  selector: 'app-rendering-area-renderer',
  templateUrl: './rendering-area-renderer.component.html',
  styleUrls: ['./rendering-area-renderer.component.scss'],
})
export class RenderingAreaRenderComponent implements OnInit {
  renderings: RenderingValue[] = [];
  renderingLookup: Rendering[] = [];
  innerField: ContentField;
  @Input()
  get field(): ContentField {
    return this.innerField;
  }
  set field(val: ContentField) {
    this.innerField = val;
    if (val.value) {
      this.renderings = val.value.Renderings;
    } else {
      this.renderings = [];
    }
    this.fieldChange.emit(val);
  }
  @Output()
  fieldChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() isEditable = true;
  @Input() websiteId: string;
  @Input() contentId: string;

  constructor(
    private dialogService: MatDialog,
    private renderingService: RenderingService
  ) {}

  openDialog() {
    const ref = this.dialogService.open(RenderingSelectorDialogComponent, {
      minWidth: 600,
    });
    ref.componentInstance.load(
      (renderingVal) => {
        this.renderings.push(renderingVal);
        this.innerField.value = {
          Renderings: this.renderings,
        }
        this.fieldChange.emit(this.innerField);
      },
      this.websiteId,
      this.contentId
    );
  }

  ngOnInit(): void {
    this.renderingService
      .getRenderings({
        IsPageRendering: false,
        WebsiteId: this.websiteId,
        PageSize: 1000,
      })
      .subscribe((res) => {
        this.renderingLookup = res.List;
      });
  }

  getRenderingName(id: string): string {
    return this.renderingLookup.find((r) => r.Id === id)?.RenderingName;
  }
}
