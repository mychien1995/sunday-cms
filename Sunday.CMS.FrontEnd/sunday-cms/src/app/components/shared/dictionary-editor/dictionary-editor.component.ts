import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
@Component({
  selector: 'app-dictionary-editor',
  templateUrl: './dictionary-editor.component.html',
})
export class DictionaryEditorComponent implements OnInit {
  ngOnInit(): void {}
  innerValue: KeyValuePair[] = [];
  @Input()
  get dictionary(): any {
    const obj = {};
    this.innerValue
      .filter((kv) => kv.Key && kv.Value)
      .forEach((item) => (obj[item.Key] = item.Value));
    return obj;
  }
  set dictionary(val: any) {
    this.innerValue = [];
    if (val && Object.keys(val).length !== 0) {
      for (const prop in val) {
        if (val.hasOwnProperty(prop)) {
          this.innerValue.push(<KeyValuePair>{ Key: prop, Value: val[prop] });
        }
      }
    }
    this.innerValue.push(<KeyValuePair>{ Key: null, Value: null });
    this.dictionaryChange.emit(val);
  }

  @Output()
  dictionaryChange: EventEmitter<any> = new EventEmitter<any>();

  onBlur(keyValue: KeyValuePair): void {
    const last = this.innerValue[this.innerValue.length - 1];
    if (
      (!keyValue.Key || keyValue.Key.trim().length === 0) &&
      keyValue !== last
    ) {
      const index = this.innerValue.indexOf(keyValue);
      this.innerValue.splice(index, 1);
    }
    if (keyValue.Key && keyValue.Value) {
      const param = {};
      this.innerValue
        .filter((kv) => kv.Key && kv.Value)
        .forEach((item) => (param[item.Key] = item.Value));
      this.dictionaryChange.emit(param);
    }
  }
}

class KeyValuePair {
  Key: any;
  Value: any;
}
