import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ModuleModel, ModuleListApiResponse } from '@models/index';
import { ApiUrl } from '@core/constants';
import { ApiService } from '@services/api.service';
import { ApiHelper } from '@services/api.helper';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class ModuleService {
  constructor(private apiService: ApiService) {}

  getModules(): Observable<ModuleListApiResponse> {
    return this.apiService
      .get(ApiUrl.Modules.Get)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
