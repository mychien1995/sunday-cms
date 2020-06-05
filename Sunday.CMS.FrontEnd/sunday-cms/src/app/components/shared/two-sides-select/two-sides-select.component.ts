import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ViewEncapsulation,
} from '@angular/core';

@Component({
  selector: 'app-two-sides-select',
  templateUrl: './two-sides-select.component.html',
  styleUrls: ['./two-sides-select.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class TwoSideSelectComponent implements OnInit {
  @Input() datasource?: any[];
  modelValues?: any[];
  @Output() valuesChange: EventEmitter<any> = new EventEmitter();

  @Input() bindValue?: string;
  @Input() bindLabel?: string;
  @Input() nested: boolean;
  @Input() nestedChild?: string;
  @Input() nestedLabel?: string;

  selectedValues: any[] = [];
  deselectedValues: any[] = [];

  @Input()
  get values(): any[] {
    return this.modelValues;
  }

  set values(val: any[]) {
    this.modelValues = val;
    this.valuesChange.emit(this.modelValues);
  }

  get actualDatasource(): any[] {
    if (this.nested) {
      const arr = this.datasource.map((c) => c[this.nestedChild]);
      if (arr.length === 0) {
        return [];
      }
      if (arr.length === 1) {
        return arr[0][this.nestedChild];
      }
      return arr.reduce((a, b) => {
        return (a ?? []).concat(b ?? []);
      });
    }
    return this.datasource;
  }
  get selectedItems() {
    return this.actualDatasource.filter(
      (c) =>
        this.modelValues.filter(
          (s) => s.toString() === c[this.bindValue].toString()
        ).length > 0
    );
  }

  get availableItems() {
    return this.actualDatasource.filter(
      (c) =>
        this.modelValues.filter(
          (s) => s.toString() === c[this.bindValue].toString()
        ).length === 0
    );
  }

  constructor() {}

  ngOnInit(): void {}

  selectItems(): void {
    this.selectedValues.forEach((item) => {
      this.modelValues.push(item);
    });
    this.selectedValues = [];
    this.valuesChange.emit(this.modelValues);
  }

  selectSingleItem(itemId: any): void {
    this.modelValues.push(itemId);
    this.valuesChange.emit(this.modelValues);
    const index = this.selectedValues.indexOf(itemId);
    if (index > -1) {
      this.selectedValues.splice(index, 1);
    }
  }

  deselectSingleItem(itemId: any): void {
    let index = this.modelValues.indexOf(itemId);
    if (index > -1) {
      this.modelValues.splice(index, 1);
    }
    this.valuesChange.emit(this.modelValues);
    index = this.deselectedValues.indexOf(itemId);
    if (index > -1) {
      this.deselectedValues.splice(index, 1);
    }
  }

  deselectItems(): void {
    this.deselectedValues.forEach((item) => {
      const index = this.modelValues.indexOf(item);
      if (index > -1) {
        this.modelValues.splice(index, 1);
      }
    });
    this.deselectedValues = [];
    this.valuesChange.emit(this.modelValues);
  }
}
