import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { ContentField, TemplateField } from '@models/index';

@Component({
  selector: 'app-multiline-text-renderer',
  templateUrl: './multiline-text-renderer.component.html',
})
export class MultilineTextRendererComponent implements OnInit {
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

  ngOnInit(): void {}
}
