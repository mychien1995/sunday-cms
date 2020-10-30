import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ViewEncapsulation,
  TemplateRef,
} from '@angular/core';

@Component({
  selector: 'app-two-sides-select-search',
  templateUrl: './two-sides-select-search.component.html',
  styleUrls: ['./two-sides-select-search.component.scss'],
})
export class TwoSideSelectWithSearchComponent implements OnInit {
  innerDatasource: any[] = [];
  innerValues: any[] = [];
  @Input()
  get datasource() {
    return this.innerDatasource;
  }
  set datasource(val: any[]) {
    this.innerDatasource = val;
    this.setDisplayValues();
    this.datasourceChange.emit(val);
  }
  @Output() datasourceChange: EventEmitter<any> = new EventEmitter();
  @Input()
  get values() {
    return this.innerValues;
  }
  set values(val: any[]) {
    this.innerValues = val;
    this.setDisplayValues();
    this.valuesChange.emit(val);
  }
  @Output() valuesChange: EventEmitter<any> = new EventEmitter();

  get availableOptions(): any[] {
    return this.searchFn(this.innerDatasource, this.query).filter(
      (d) =>
        this.innerValues.filter(
          (v) => v.toString() === d[this.bindValue].toString()
        ).length === 0
    );
  }

  @Input() searchFn: (data: any[], text: string) => any[];
  @Input() childTemplate: TemplateRef<any>;
  @Input() placeholder: string;
  @Input() bindValue: string;
  @Input() multiselect = false;
  displayValues: any[] = [];
  query: string;

  ngOnInit(): void {}

  setDisplayValues(): void {
    this.displayValues = this.innerValues.map((v) =>
      this.innerDatasource.find(
        (d) => d[this.bindValue].toString() === v.toString()
      )
    ).filter(v => v);
  }

  selectItem(item: any) {
    if (!this.multiselect) {
      this.innerDatasource.forEach((i) => (i.Two_Side_Active = false));
    }
    item.Two_Side_Active = !item.Two_Side_Active;
  }

  moveItem() {
    const selectedItems = this.innerDatasource.filter((f) => f.Two_Side_Active);
    this.innerDatasource.forEach((i) => (i.Two_Side_Active = false));
    selectedItems.forEach((i) => this.innerValues.push(i[this.bindValue]));
    this.setDisplayValues();
  }

  revertItem() {
    const selectedItems = this.displayValues.filter((f) => f.Two_Side_Active);
    selectedItems.forEach((i) => {
      const index = this.innerValues.indexOf(i[this.bindValue]);
      if (index > -1) {
        this.innerValues.splice(index, 1);
      }
    });
    this.innerDatasource.forEach((i) => (i.Two_Side_Active = false));
    this.setDisplayValues();
  }

  moveUp() {
    const displayValues = this.displayValues;
    const selectedItems = displayValues.filter((f) => f.Two_Side_Active);
    let moved = true;
    selectedItems.forEach(function (item) {
      const index = displayValues.indexOf(item);
      if (index !== 0 && moved) {
        const tmp = displayValues[index - 1];
        displayValues[index - 1] = displayValues[index];
        displayValues[index] = tmp;
      } else {
        moved = false;
      }
    });
    this.values = displayValues.map((v) => v[this.bindValue]);
  }

  moveDown() {
    const displayValues = this.displayValues;
    const selectedItems = displayValues.filter((f) => f.Two_Side_Active);
    let moved = true;
    selectedItems.forEach(function (item) {
      const index = displayValues.indexOf(item);
      if (index !== displayValues.length - 1 && moved) {
        const tmp = displayValues[index + 1];
        displayValues[index + 1] = displayValues[index];
        displayValues[index] = tmp;
      } else {
        moved = false;
      }
    });
    this.values = displayValues.map((v) => v[this.bindValue]);
  }
}
