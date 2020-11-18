import { Injectable, Output, EventEmitter } from '@angular/core';
import { ContentModel } from '@models/index';
@Injectable()
export class ContentBus {
  @Output() contentBus: EventEmitter<ContentModel> = new EventEmitter();

  contentUpdated(data: ContentModel): void {
    this.contentBus.emit(data);
  }
}
