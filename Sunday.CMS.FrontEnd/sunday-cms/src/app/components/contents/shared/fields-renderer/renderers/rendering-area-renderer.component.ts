import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ContentField, TemplateField } from '@models/index';
import { RenderingSelectorDialogComponent } from './partials/rendering-selector-dialog.component';

@Component({
  selector: 'app-rendering-area-renderer',
  templateUrl: './rendering-area-renderer.component.html',
})
export class RenderingAreaRenderComponent implements OnInit {
  innerField: ContentField;
  @Input()
  get field(): ContentField {
    return this.innerField;
  }
  set field(val: ContentField) {
    this.innerField = val;
    this.fieldChange.emit(val);
  }
  @Output()
  fieldChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() isEditable = true;
  @Input() websiteId: string;
  @Input() contentId: string;

  constructor(private dialogService: MatDialog) {}

  openDialog() {
    const ref = this.dialogService.open(RenderingSelectorDialogComponent, {
      minWidth: 600,
    });
    ref.componentInstance.load(
      (renderingVal) => {
        console.log(renderingVal);
        this.innerField.value = renderingVal;
      },
      this.websiteId,
      this.contentId
    );
  }

  ngOnInit(): void {}
}
