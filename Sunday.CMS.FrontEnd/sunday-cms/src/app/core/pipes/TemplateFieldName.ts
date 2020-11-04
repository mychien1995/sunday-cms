import { Pipe, PipeTransform } from '@angular/core';
import { TemplateField } from '@models/index';

@Pipe({ name: 'templateName' })
export class TemplateFieldName implements PipeTransform {
  transform(field: TemplateField): any {
    const displayName = field.DisplayName;
    const fieldName = field.FieldName;
    return displayName && displayName.length > 0 ? displayName : fieldName;
  }
}
