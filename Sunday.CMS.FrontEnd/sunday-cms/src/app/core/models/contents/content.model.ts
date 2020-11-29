import { ApiResponse } from '@models/common/response.model';
import {
  TemplateField,
  TemplateItem,
} from '@models/template-management/template.model';

export class ContentField {
  field: TemplateField = new TemplateField();
  value?: any;
}

export class ContentModel extends ApiResponse {
  Id: string;
  Name: string;
  DisplayName: string;
  ParentId: string;
  Path: string;
  ParentType: number;
  TemplateId: string;
  CreatedDate: Date;
  UpdatedDate: Date;
  PublishedDate?: Date;
  CreatedBy: string;
  UpdatedBy: string;
  PublishedBy?: string;
  SortOrder: number;
  SelectedVersion?: string;
  NamePath?: string;
  Template: TemplateItem = new TemplateItem();
  Versions: ContentVersion[] = [];
  Fields: ContentFieldItem[] = [];
}

export class ContentVersion {
  VersionId?: string;
  Version?: number;
  IsActive?: boolean;
  Status?: number;
}

export class ContentFieldItem {
  Id: string;
  FieldValue?: string;
  TemplateFieldId: string;
  TemplateFieldCode: number;
}
