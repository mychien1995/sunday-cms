import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-icon-picker-dialog',
  templateUrl: './icon-picker-dialog.component.html',
})
export class IconPickerDialogComponent implements OnInit {
  iconLookup: any[];
  selectedIcon: string;
  callback: (icon: string) => any;
  ngOnInit(): void {}

  load(iconLookup: any[], callback: (icon: string) => any) {
    this.iconLookup = iconLookup;
    this.callback = callback;
  }

  onSubmit(): void {
    this.callback(this.selectedIcon);
  }
}
