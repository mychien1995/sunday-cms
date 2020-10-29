import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { default as Icon } from 'app/../assets/config/icons.json';
@Injectable()
export class IconService {
  colorConfig: any;
  constructor(private httpClient: HttpClient) {}
  getIcons(): any[] {
    const list: any = [];
    const icons = Icon;
    // tslint:disable-next-line: forin
    for (const key in icons) {
      list.push({
        code: key,
        icon: icons[key],
      });
    }
    return list;
  }

  getIcon(name: string): any {
    const icons = Icon;
    return icons[name];
  }
}
