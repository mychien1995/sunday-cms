import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'beautifyPath' })
export class BeautifyPathPipe implements PipeTransform {
  transform(input: string): any {
    if (!input) {
      return '';
    }
    return input.split('/').join(' / ');
  }
}
