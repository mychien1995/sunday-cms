import { OnInit, Component } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ContentTreeNode } from '@models/index';
import { InitialContentCreatorComponent } from './initial-content-creator.component';

@Component({
  selector: 'app-template-selector-dialog',
  templateUrl: './template-selector-dialog.component.html',
  styleUrls: ['./template-selector-dialog.component.scss'],
})
export class TemplateSelectorDialogComponent implements OnInit {
  selectedTemplates: string[] = [];
  parent: ContentTreeNode;
  constructor(
    private dialogService: MatDialog,
    private dialogRef: MatDialogRef<TemplateSelectorDialogComponent>
  ) {}
  ngOnInit(): void {}

  load(parent: ContentTreeNode) {
    this.parent = parent;
  }
  onSubmit(): void {
    if (this.selectedTemplates.length > 0) {
      const ref = this.dialogService.open(InitialContentCreatorComponent, {
        minWidth: 1200,
        disableClose: true,
      });
      ref.componentInstance.load(this.selectedTemplates[0], this.parent);
      this.dialogRef.close();
    }
  }
}
