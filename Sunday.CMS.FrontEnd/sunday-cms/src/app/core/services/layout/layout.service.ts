import { Injectable, Output, EventEmitter } from '@angular/core';

@Injectable()
export class LayoutService {
  @Output() layoutBus: EventEmitter<any> = new EventEmitter();

  layoutUpdated(data: any): void {
    this.layoutBus.emit(data);
  }
}
