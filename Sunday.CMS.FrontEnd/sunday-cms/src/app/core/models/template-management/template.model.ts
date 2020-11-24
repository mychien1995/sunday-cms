import { EntityAccess } from '@models/common/entity.access.model';
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
  IsAbstract: boolean;
  Fields: TemplateField[] = [];
  Access: EntityAccess;
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
  IsRequired?: number;
  IsPlaceholder?: boolean;
  TypeName?: string;
  get DisplayText(): string {
    return this.DisplayName && this.DisplayName.length > 0
      ? this.DisplayName
      : this.FieldName;
  }
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
  Layout: string;
}
