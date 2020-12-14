import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { IconService } from '@services/index';
import { IconPickerDialogComponent } from './icon-picker-dialog.component';

@Component({
  selector: 'app-icon-picker',
  templateUrl: './icon-picker.component.html',
})
export class IconPickerComponent implements OnInit {
  iconLookup: any[] = [];
  innerValue: string;
  @Input()
  get value(): string {
    return this.innerValue;
  }
  set value(val: string) {
    this.innerValue = val;
    this.valueChange.emit(val);
  }
  @Output()
  valueChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() required: boolean;

  constructor(
    private iconService: IconService,
    private dialogService: MatDialog
  ) {
    this.iconLookup = this.iconService.getIcons();
  }

  openDialog(): void {
    const ref = this.dialogService.open(IconPickerDialogComponent, {
      minWidth: 600,
    });
    ref.componentInstance.load(this.iconLookup, (icon) => {
      this.value = icon;
    });
  }
  getCurrentIcon(): any {
    return this.iconLookup.find((i) => i.Code === this.innerValue);
  }

  ngOnInit(): void {}
}
