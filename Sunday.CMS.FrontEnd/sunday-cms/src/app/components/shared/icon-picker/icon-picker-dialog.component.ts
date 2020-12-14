import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-icon-picker-dialog',
  templateUrl: './icon-picker-dialog.component.html',
  styleUrls: ['./icon-picker-dialog.component.scss'],
})
export class IconPickerDialogComponent implements OnInit {
  iconLookup: any[];
  selectedIcon: string;
  callback: (icon: string) => any;
  ngOnInit(): void {}
  constructor(private dialogService: MatDialog) {}

  load(iconLookup: any[], callback: (icon: string) => any) {
    this.iconLookup = iconLookup;
    this.callback = callback;
  }

  onSubmit(): void {
    if (this.selectedIcon) {
      this.callback(this.selectedIcon);
    }
    this.dialogService.closeAll();
  }

  select(icon: any): void {
    this.selectedIcon = icon.code;
  }
}
