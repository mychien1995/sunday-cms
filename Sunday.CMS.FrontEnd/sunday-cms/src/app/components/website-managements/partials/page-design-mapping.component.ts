import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TemplateItem, Rendering } from '@models/index';

@Component({
  selector: 'app-page-design-mapping',
  templateUrl: './page-design-mapping.component.html',
})
export class PageDesignMappingComponent implements OnInit {
  @Input()
  templateLookup: TemplateItem[] = [];
  @Input()
  renderingLookup: Rendering[] = [];

  innerMapping: KeyValuePair[] = [];
  @Input()
  get mapping(): any {
    const obj = {};
    this.innerMapping
      .filter((kv) => kv.Key && kv.Value)
      .forEach((item) => (obj[item.Key] = item.Value));
    return obj;
  }
  set mapping(val: any) {
    this.innerMapping = [];
    if (val && Object.keys(val).length !== 0) {
      for (const prop in val) {
        if (val.hasOwnProperty(prop)) {
          this.innerMapping.push(<KeyValuePair>{ Key: prop, Value: val[prop] });
        }
      }
    } else {
      this.innerMapping.push(<KeyValuePair>{ Key: null, Value: null });
    }
    this.mappingChange.emit(val);
  }
  @Output()
  mappingChange: EventEmitter<any> = new EventEmitter<any>();

  ngOnInit(): void {}

  updateMapping() {
    if (this.mapping && Object.keys(this.mapping).length !== 0) {
      this.mappingChange.emit(this.mapping);
    }
  }
}

class KeyValuePair {
  Key: any;
  Value: any;
}
