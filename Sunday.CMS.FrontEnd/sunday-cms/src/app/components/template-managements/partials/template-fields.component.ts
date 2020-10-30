import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TemplateManagementService } from '@services/index';
import { FieldType, TemplateItem, TemplateField } from '@models/index';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-template-fields',
  templateUrl: './template-fields.component.html',
  providers: [NgbModalConfig, NgbModal],
})
export class TemplateFieldComponent implements OnInit {
  innerTemplate: TemplateItem = new TemplateItem();
  @Input() submitted: boolean;
  @Input()
  get template(): TemplateItem {
    const fields = this.innerTemplate.Fields;
    if (fields.filter((f) => f.IsPlaceholder).length === 0) {
      this.addPlaceholder();
    }
    return this.innerTemplate;
  }
  set template(val: TemplateItem) {
    this.innerTemplate = val;
    this.templateChange.emit(this.innerTemplate);
  }
  @Output() templateChange: EventEmitter<TemplateItem> = new EventEmitter();

  get templateFields(): TemplateField[] {
    return this.template.Fields.filter(
      (c) => !c.IsPlaceholder && c.FieldName && c.FieldName.trim().length > 0
    );
  }

  fieldTypeLookup: FieldType[] = [];

  constructor(private templateService: TemplateManagementService) {
    this.templateService.getFieldTypes().subscribe((res) => {
      this.fieldTypeLookup = res.FieldTypes;
    });
  }

  addRow(item: TemplateField): void {
    const name = item.FieldName;
    if (name && name.trim().length > 0) {
      item.IsPlaceholder = false;
      this.addPlaceholder();
    }
  }

  moveUp(index: number) {
    const fields = this.innerTemplate.Fields;
    const temp = fields[index - 1];
    fields[index - 1] = fields[index];
    fields[index] = temp;
  }

  moveDown(index: number) {
    const fields = this.innerTemplate.Fields;
    const temp = fields[index + 1];
    fields[index + 1] = fields[index];
    fields[index] = temp;
  }

  removeField(index: number): void {
    this.innerTemplate.Fields = this.innerTemplate.Fields.splice(index, 1);
  }

  addPlaceholder(): void {
    this.innerTemplate.Fields.push(<TemplateField>{
      IsPlaceholder: true,
      FieldType: 1,
    });
  }
  ngOnInit(): void {}

  isValid(): boolean {
    return (
      this.template.Fields.filter(
        (f) => !f.IsPlaceholder && (!f.FieldName || f.FieldName.trim().length === 0)
      ).length === 0
    );
  }
}
