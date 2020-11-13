import {
  MapArrayPipe,
  JoinStringPipe,
  FilterEmptyStringPipe,
  EpochToDatetimePipe,
  TemplateFieldName,
  BeautifyPathPipe,
} from '@core/pipes/index';

export const SharedPipes = [
  MapArrayPipe,
  JoinStringPipe,
  FilterEmptyStringPipe,
  EpochToDatetimePipe,
  TemplateFieldName,
  BeautifyPathPipe,
];
