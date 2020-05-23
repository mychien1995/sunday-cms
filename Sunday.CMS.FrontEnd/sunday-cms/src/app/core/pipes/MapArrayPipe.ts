import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'mapArray'})
export class MapArrayPipe implements PipeTransform {
  transform(input: any[], key : string): any  {
    return input.map(value => value[key]);
  }
}