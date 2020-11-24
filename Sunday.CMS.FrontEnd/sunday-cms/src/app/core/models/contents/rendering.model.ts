import { EntityAccess } from '@models/common/entity.access.model';
import { ApiResponse } from '@models/common/response.model';
export class Rendering extends ApiResponse {
  Id: string;
  RenderingName: string;
  Controller: string;
  Action: string;
  IsPageRendering: boolean;
  IsRequireDatasource: boolean;
  DatasourceTemplate: string;
  DatasourceLocation: string;
  CreatedDate: Date;
  UpdatedDate: Date;
  CreatedBy: string;
  UpdatedBy: string;
  Access: EntityAccess;
  Expand: boolean;
}
