import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ContentField, LinkValue, TemplateField } from '@models/index';
import { LinkEditorDialogComponent } from './partials/link-editor-dialog.component';

@Component({
  selector: 'app-link-renderer',
  templateUrl: './link-renderer.component.html',
})
export class LinkRendererComponent implements OnInit {
  innerLink: LinkValue = new LinkValue();
  innerField: ContentField;
  @Input()
  get field(): ContentField {
    return this.innerField;
  }
  set field(val: ContentField) {
    this.innerField = val;
    if (val.value) {
      this.innerLink = <LinkValue>val.value;
    }
    this.fieldChange.emit(val);
  }
  @Output()
  fieldChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() isEditable = true;

  constructor(private dialogService: MatDialog) {}
  ngOnInit(): void {}

  openDialog(): void {
    const ref = this.dialogService.open(LinkEditorDialogComponent);
    ref.componentInstance.load(this.innerLink, (link) => {
      this.innerField.value = link;
      this.innerLink = link;
      this.fieldChange.emit(this.innerField);
    });
  }
}
