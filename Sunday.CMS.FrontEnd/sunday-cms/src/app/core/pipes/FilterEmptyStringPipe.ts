import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'filterEmptyString' })
export class FilterEmptyStringPipe implements PipeTransform {
  transform(input: string[]): any {
    return input.filter((x) => x && x.trim().length > 0);
  }
}
