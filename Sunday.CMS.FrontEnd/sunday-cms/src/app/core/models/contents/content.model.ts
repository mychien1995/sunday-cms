import { TemplateField } from '@models/template-management/template.model';

export class ContentField {
  field: TemplateField = new TemplateField();
  value?: any;
}

export class ContentModel {
  Id: string;
  Name: string;
  DisplayName: string;
  ParentId: string;
  ParentType: number;
  TemplateId: string;
  CreatedDate: Date;
  UpdatedDate: Date;
  PublishedDate?: Date;
  CreatedBy: string;
  UpdatedBy: string;
  PublishedBy?: string;
  SortOrder: number;
  ActiveVersion: string;
  Versions: ContentVersion[] = [];
  Fields: ContentFieldItem[] = [];
}

export class ContentVersion {
  VersionId: string;
  Version: number;
  IsActive: boolean;
}

export class ContentFieldItem {
  Id: string;
  FieldValue?: string;
  TemplateFieldId: string;
}
