import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IconService } from '@services/index';
import { TemplateItem } from '@models/index';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-template-info',
  templateUrl: './template-info.component.html',
  providers: [NgbModalConfig, NgbModal],
})
export class TemplateInfoComponent implements OnInit {
  public dataForm: FormGroup = new FormGroup({});
  innerTemplate: TemplateItem = new TemplateItem();
  @Input() submitted: boolean;
  @Input()
  get template(): TemplateItem {
    return this.innerTemplate;
  }
  set template(val: TemplateItem) {
    this.innerTemplate = val;
    this.templateChange.emit(this.innerTemplate);
  }
  @Output() templateChange: EventEmitter<TemplateItem> = new EventEmitter();

  iconLookup: any[] = [];
  constructor(private iconService: IconService) {
    this.iconLookup = this.iconService.getIcons();
  }

  buildForm(): void {
    this.dataForm = new FormGroup({
      TemplateName: new FormControl(this.innerTemplate.TemplateName, [
        Validators.required,
      ]),
      Icon: new FormControl(this.innerTemplate.Icon, [Validators.required]),
    });
  }

  ngOnInit(): void {
    this.buildForm();
  }

  isValid(): boolean {
    return this.dataForm.valid;
  }
}
