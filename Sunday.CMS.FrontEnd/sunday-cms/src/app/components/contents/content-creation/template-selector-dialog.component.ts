import { OnInit, Component } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ContentService, TemplateManagementService } from '@services/index';
import { ContentTreeNode, TemplateItem } from '@models/index';
import { InitialContentCreatorComponent } from './initial-content-creator.component';

@Component({
  selector: 'app-template-selector-dialog',
  templateUrl: './template-selector-dialog.component.html',
  styleUrls: ['./template-selector-dialog.component.scss'],
})
export class TemplateSelectorDialogComponent implements OnInit {
  selectedTemplates: string[] = [];
  parent: ContentTreeNode;
  insertOptions: TemplateItem[] = [];
  showInsertOptions = false;
  hasInsertOptions = false;
  isLoading = true;
  onClose: (id: string) => any;
  constructor(
    private dialogService: MatDialog,
    private dialogRef: MatDialogRef<TemplateSelectorDialogComponent>,
    private contentService: ContentService,
    private templateService: TemplateManagementService
  ) {}
  ngOnInit(): void {}

  load(parent: ContentTreeNode, onClose: (id: string) => any) {
    this.parent = parent;
    if (this.parent.Type != 3) {
      this.isLoading = false;
      return;
    }
    this.contentService
      .get(this.parent.Id)
      .toPromise()
      .then((res) =>
        this.templateService
          .getTemplateById(res.TemplateId)
          .toPromise()
          .then((template) => {
            if (template.InsertOptions.length === 0) {
              this.isLoading = false;
              return;
            }
            this.templateService
              .getTemplates({ IncludeIds: template.InsertOptions })
              .subscribe((templateList) => {
                if (templateList.Templates.length > 0) {
                  this.insertOptions = templateList.Templates;
                  this.hasInsertOptions = true;
                  this.showInsertOptions = true;
                  this.isLoading = false;
                }
              });
          })
      );
    this.onClose = onClose;
  }
  onSubmit(): void {
    if (this.selectedTemplates.length > 0) {
      const ref = this.dialogService.open(InitialContentCreatorComponent, {
        minWidth: 1200,
        disableClose: true,
      });
      ref.componentInstance.load(
        this.selectedTemplates[0],
        this.parent,
        this.onClose
      );
      this.dialogRef.close();
    }
  }
}
