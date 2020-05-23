import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'joinString'})
export class JoinStringPipe implements PipeTransform {
  transform(input: any[], delimeter : string): any  {
    return input.join(delimeter);
  }
}