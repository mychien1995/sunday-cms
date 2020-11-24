import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { TemplateItem } from '@models/index';
import { IconService, TemplateManagementService } from '@services/index';

@Component({
  selector: 'app-template-selector',
  templateUrl: './template-selector.component.html',
  styleUrls: ['./template-selector.component.scss'],
})
export class TemplateSelectorComponent implements OnInit {
  innerSelectedTemplates: string[] = [];
  @Input()
  get selectedTemplates(): string[] {
    return this.innerSelectedTemplates;
  }
  set selectedTemplates(val: string[]) {
    this.innerSelectedTemplates = val;
    this.selectedTemplatesChange.emit(val);
  }
  @Output()
  selectedTemplatesChange: EventEmitter<any> = new EventEmitter<any>();

  @Input()
  templateLookup: TemplateItem[] = [];
  query: any = { PageSize: 1000 };
  @Input() multiple = false;
  isLoading = false;

  get displayTemplates(): TemplateItem[] {
    return this.templateLookup.filter(
      (c) =>
        !this.query.Text ||
        this.query.Text.length === 0 ||
        c.Id === this.query.Text.toLowerCase() ||
        c.TemplateName.toLowerCase().indexOf(
          this.query.Text.trim().toLowerCase()
        ) > -1
    );
  }

  constructor(
    private iconService: IconService,
    private templateService: TemplateManagementService
  ) {
    this.loadTemplates();
  }

  getIcon(code: string): string {
    return this.iconService.getIcon(code);
  }

  loadTemplates() {
    if (this.templateLookup.length !== 0) {
      return;
    }
    this.isLoading = true;
    this.templateService.getTemplates(this.query).subscribe(
      (res) => {
        if (this.templateLookup.length === 0) {
          this.templateLookup = res.Templates;
        }
        this.isLoading = false;
      },
      (ex) => (this.isLoading = false)
    );
  }

  select(template: TemplateItem) {
    const index = this.innerSelectedTemplates.indexOf(
      this.innerSelectedTemplates.find((c) => c === template.Id)
    );
    if (index > -1) {
      this.innerSelectedTemplates.splice(index, 1);
    } else {
      if (!this.multiple) {
        this.innerSelectedTemplates = [];
      }
      this.innerSelectedTemplates.push(template.Id);
    }
    this.selectedTemplates = this.innerSelectedTemplates;
  }

  selected(template: TemplateItem) {
    return this.innerSelectedTemplates.find((c) => c === template.Id);
  }

  ngOnInit(): void {}
}
