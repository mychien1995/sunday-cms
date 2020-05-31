import { ApiResponse } from '@models/common/response.model';

export class ModuleModel {
  ID?: number;
  ModuleName?: string;
  ModuleCode?: string;
}

export class ModuleListApiResponse extends ApiResponse {
  Total?: number;
  Modules: ModuleModel[] = [];
}
