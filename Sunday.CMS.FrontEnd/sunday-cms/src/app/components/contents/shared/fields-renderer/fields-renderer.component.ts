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
  @Input() formSubmitted = false;
  @Input() isEditable = true;
  @Input() websiteId: string;
  @Input() contentId: string;

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

  isValid(field: ContentField): boolean {
    if (field.field.IsRequired) {
      if (field.field.FieldType === 6) {
        return (
          !this.emptyString(field.value.LinkText) &&
          !this.emptyString(field.value.Url)
        );
      }
      if (field.field.FieldType === 9) {
        return field.value && field.value.length > 0;
      }
      return !this.emptyString(field.value);
    }
    return true;
  }

  emptyString = (str: any) => !str || str.trim().length === 0;

  ngOnInit(): void {}
}
