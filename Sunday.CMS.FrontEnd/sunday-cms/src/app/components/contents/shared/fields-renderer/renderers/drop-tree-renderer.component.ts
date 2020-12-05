import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ContentService } from '@services/index';
import {
  ContentField,
  ContentModel,
  LinkValue,
  TemplateField,
} from '@models/index';
import { DatasourceSelectorDialogComponent } from './partials/datasource-selector.component';
import { LinkEditorDialogComponent } from './partials/link-editor-dialog.component';

@Component({
  selector: 'app-drop-tree-renderer',
  templateUrl: './drop-tree-renderer.component.html',
})
export class DropTreeRendererComponent implements OnInit {
  innerField: ContentField;
  @Input()
  get field(): ContentField {
    return this.innerField;
  }
  set field(val: ContentField) {
    this.innerField = val;
    if (this.innerField.value) {
      this.contentService.get(this.innerField.value).subscribe((res) => {
        this.content = res;
      });
    }
    this.fieldChange.emit(val);
  }
  @Output()
  fieldChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() isEditable = true;
  @Input() websiteId: string;
  content: ContentModel;

  constructor(
    private dialogService: MatDialog,
    private contentService: ContentService
  ) {}
  ngOnInit(): void {}

  openDialog(): void {
    const ref = this.dialogService.open(DatasourceSelectorDialogComponent, {
      minWidth: 600,
    });
    ref.componentInstance.load(this.field.field.Properties, this.websiteId, (contentId) => {
      this.innerField.value = contentId;
      this.field = this.innerField;
    });
  }
}
