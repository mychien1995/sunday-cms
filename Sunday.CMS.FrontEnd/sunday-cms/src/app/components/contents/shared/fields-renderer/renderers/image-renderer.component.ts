import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { ContentField, TemplateField } from '@models/index';

@Component({
  selector: 'app-image-renderer',
  templateUrl: './image-renderer.component.html',
})
export class ImageRendererComponent implements OnInit {
  innerField: ContentField;
  @Input()
  get field(): ContentField {
    return this.innerField;
  }
  set field(val: ContentField) {
    this.innerField = val;
    this.fieldsChange.emit(val);
  }
  @Output()
  fieldsChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() isEditable = true;

  ngOnInit(): void {}
}
