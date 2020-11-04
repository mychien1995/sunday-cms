import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { TemplateManagementService } from '@services/index';
import { ContentField, FieldType, TemplateField } from '@models/index';

@Component({
  selector: 'app-fields-renderer',
  templateUrl: './fields-renderer.component.html',
  styleUrls: ['./fields-renderer.component.scss'],
})
export class FieldsRendererComponent implements OnInit {
  innerFields: ContentField[] = [];
  @Input()
  get fields(): ContentField[] {
    return this.innerFields;
  }
  set fields(val: ContentField[]) {
    this.innerFields = val;
    this.fieldsChange.emit(val);
  }
  @Output()
  fieldsChange: EventEmitter<any> = new EventEmitter<any>();
  fieldTypes: FieldType[] = [];
  isLoading = false;

  constructor(private templateService: TemplateManagementService) {
    this.isLoading = true;
    this.templateService.getFieldTypes().subscribe((res) => {
      this.fieldTypes = res.FieldTypes;
      this.isLoading = false;
    });
  }

  getTypeName(id: number): string {
    return this.fieldTypes.find((f) => f.Id === id)?.Layout;
  }
  ngOnInit(): void {}
}
