import { ApiResponse } from '@models/common/response.model';

export class ModuleModel {
  Id?: string;
  ModuleName?: string;
  ModuleCode?: string;
  Features?: FeatureItem[];
}

export class ModuleListApiResponse extends ApiResponse {
  Total?: number;
  Modules: ModuleModel[] = [];
}

export class FeatureItem {
  Id?: string;
  FeatureCode?: string;
  FeatureName?: string;
}

export class FeatureListApiResponse extends ApiResponse {
  Total?: number;
  Features: FeatureItem[] = [];
}
