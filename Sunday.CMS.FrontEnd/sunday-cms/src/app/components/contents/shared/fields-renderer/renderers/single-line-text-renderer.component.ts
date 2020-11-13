import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { ContentField, TemplateField } from '@models/index';

@Component({
  selector: 'app-single-line-text-renderer',
  templateUrl: './single-line-text-renderer.component.html',
})
export class SingleLineTextRendererComponent implements OnInit {
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
