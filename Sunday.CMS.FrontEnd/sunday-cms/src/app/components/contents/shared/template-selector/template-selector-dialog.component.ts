import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { TemplateItem } from '@models/index';
import { IconService, TemplateManagementService } from '@services/index';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-template-selector-inline-dialog',
  templateUrl: './template-selector-dialog.component.html',
})
export class InlineTemplateSelectorDialogComponent implements OnInit {
  selectedTemplates: string[] = [];
  templateLookup: TemplateItem[] = [];
  callback: (selected: string[]) => void;
  ngOnInit(): void {}
  constructor(private dialogService: MatDialog) {}

  load(
    templateLookup: TemplateItem[],
    selectedTemplates: string[],
    callback: (selected: string[]) => void
  ): void {
    this.selectedTemplates = [...selectedTemplates];
    this.templateLookup = templateLookup;
    this.callback = callback;
  }

  onSubmit(): void {
    if (this.selectedTemplates.length > 0) {
      this.callback(this.selectedTemplates);
      this.dialogService.closeAll();
    }
  }
}
