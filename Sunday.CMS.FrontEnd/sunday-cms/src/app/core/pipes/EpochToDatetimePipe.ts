import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({ name: 'unixToDatetime' })
export class EpochToDatetimePipe implements PipeTransform {

  transform(epochTime: number, format: string = 'DD/MM/YYYY'): any {
    const date = new Date(epochTime);
    const dateStr = moment(date).format(format);
    return dateStr;
  }
}
