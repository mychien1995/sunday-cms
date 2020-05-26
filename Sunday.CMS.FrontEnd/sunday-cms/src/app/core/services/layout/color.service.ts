import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { default as Color } from 'app/../assets/config/colors.json';
@Injectable()
export class ColorService {
  colorConfig: any;
  constructor(private httpClient: HttpClient) {}
  getColors(): any[] {
    const list: any = [];
    const colors = Color;
    for (var key in colors) {
      list.push({
        code: key,
        name: colors[key] && colors[key].name ? colors[key].name : key,
        hex: colors[key].hex,
      });
    }
    return list;
  }

  getColor(name: string): any {
    const colors = Color;
    return colors[name];
  }
}
