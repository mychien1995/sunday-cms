import { ApiResponse } from '@models/common/response.model';

export class TemplateItem extends ApiResponse {
  Id: string;
  TemplateName: string;
  Icon: string;
  BaseTemplateIds: string[] = [];
  CreatedDate: Date;
  UpdatedDate: Date;
  CreatedBy: string;
  UpdatedBy: string;
  Fields: TemplateField[] = [];
}
export class TemplateField {
  Id?: string;
  FieldName?: string;
  DisplayName?: string;
  FieldType?: number;
  Title?: string;
  IsUnversioned?: boolean;
  Properties?: string;
  SortOrder?: number;
  IsPlaceholder?: boolean;
}

export class TemplateList extends ApiResponse {
  Total: number;
  Templates: TemplateItem[] = [];
}

export class FieldTypeList extends ApiResponse {
  FieldTypes: FieldType[] = [];
}

export class FieldType {
  Id: number;
  Name: string;
}
