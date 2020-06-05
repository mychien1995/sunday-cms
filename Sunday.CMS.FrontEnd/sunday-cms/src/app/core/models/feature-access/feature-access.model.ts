import { ApiResponse } from '@models/common/response.model';

export class ModuleModel {
  ID?: number;
  ModuleName?: string;
  ModuleCode?: string;
  Features?: FeatureItem[];
}

export class ModuleListApiResponse extends ApiResponse {
  Total?: number;
  Modules: ModuleModel[] = [];
}

export class FeatureItem {
  ID?: number;
  FeatureCode?: string;
  FeatureName?: string;
}

export class FeatureListApiResponse extends ApiResponse {
  Total?: number;
  Features: FeatureItem[] = [];
}
