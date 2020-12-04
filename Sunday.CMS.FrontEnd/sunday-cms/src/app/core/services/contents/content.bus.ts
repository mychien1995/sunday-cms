import { Injectable, Output, EventEmitter } from '@angular/core';
import { ContentModel } from '@models/index';
@Injectable()
export class ContentBus {
  @Output() contentBus: EventEmitter<any> = new EventEmitter();

  contentUpdated(data: ContentModel): void {
    this.contentBus.emit({
      ev: 'content-updated',
      content: data,
    });
  }

  contentLinkSelect(id: string): void {
    this.contentBus.emit({
      ev: 'content-link',
      id: id,
    });
  }
}
