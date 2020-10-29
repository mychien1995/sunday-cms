import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-string-list-editor',
  templateUrl: './string-list-editor.component.html',
})
export class StringListEditorComponent implements OnInit {
  @Input() name: string;
  @Input() placeholder: string;

  modelValues: string[] = [''];
  @Input()
  get values(): any[] {
    if (this.modelValues.length === 0) {
      this.modelValues.push('');
    }
    return this.modelValues;
  }
  set values(val: any[]) {
    this.modelValues = val;
    this.valuesChange.emit(this.modelValues);
  }
  @Output() valuesChange: EventEmitter<any> = new EventEmitter();

  addValue(index: number): void {
    if (
      this.modelValues[index].trim().length === 0 &&
      index !== this.modelValues.length - 1
    ) {
      this.modelValues.splice(index, 1);
      return;
    }
    if (
      this.modelValues.filter((x) => !x || x.trim().length === 0).length > 0
    ) {
      return;
    }
    this.modelValues.push('');
  }
  trackByFn(index: any, item: any) {
    return index;
  }

  ngOnInit(): void {}
}
