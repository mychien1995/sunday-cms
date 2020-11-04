import { OnInit, Component } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { InitialContentCreatorComponent } from './initial-content-creator.component';

@Component({
  selector: 'app-template-selector-dialog',
  templateUrl: './template-selector-dialog.component.html',
  styleUrls: ['./template-selector-dialog.component.scss'],
})
export class TemplateSelectorDialogComponent implements OnInit {
  selectedTemplates: string[] = [];
  constructor(
    private dialogService: MatDialog,
    private dialogRef: MatDialogRef<TemplateSelectorDialogComponent>
  ) {}
  ngOnInit(): void {}

  onSubmit(): void {
    if (this.selectedTemplates.length > 0) {
      const ref = this.dialogService.open(InitialContentCreatorComponent, {
        minWidth: 800,
        disableClose: true,
      });
      ref.componentInstance.load(this.selectedTemplates[0]);
      this.dialogRef.close();
    }
  }
}
