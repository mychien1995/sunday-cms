import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { TemplateItem } from '@models/index';
import { IconService, TemplateManagementService } from '@services/index';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { InlineTemplateSelectorDialogComponent } from './template-selector-dialog.component';

@Component({
  selector: 'app-template-selector-button',
  templateUrl: './template-selector-button.component.html',
})
export class TemplateSelectorButtonComponent implements OnInit {
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
  readonly = false;

  templateLookup: TemplateItem[] = [];

  get displayTemplates(): TemplateItem[] {
    return this.templateLookup.filter(
      (t) => this.selectedTemplates.indexOf(t.Id) > -1
    );
  }

  constructor(
    private templateService: TemplateManagementService,
    private modalService: MatDialog
  ) {
    this.loadTemplates();
  }

  loadTemplates() {
    this.templateService.getTemplates({ PageSize: 1000 }).subscribe((res) => {
      this.templateLookup = res.Templates;
    });
  }

  openDialog() {
    if (this.readonly) {
      return;
    }
    const ref = this.modalService.open(InlineTemplateSelectorDialogComponent, {
      minWidth: 800,
      disableClose: true,
    });
    ref.componentInstance.load(
      this.templateLookup,
      this.selectedTemplates,
      (selecteds) => {
        this.selectedTemplates = selecteds;
      }
    );
  }
  ngOnInit(): void {}
}
