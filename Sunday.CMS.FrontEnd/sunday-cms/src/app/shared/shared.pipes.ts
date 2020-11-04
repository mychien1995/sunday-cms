import { MapArrayPipe, JoinStringPipe, FilterEmptyStringPipe, EpochToDatetimePipe, TemplateFieldName } from '@core/pipes/index';

export const SharedPipes = [
    MapArrayPipe,
    JoinStringPipe,
    FilterEmptyStringPipe,
    EpochToDatetimePipe,
    TemplateFieldName
];