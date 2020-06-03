import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FeatureListApiResponse } from '@models/index';
import { ApiUrl } from '@core/constants';
import { ApiService } from '@services/api.service';
import { ApiHelper } from '@services/api.helper';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class FeatureService {
  constructor(private apiService: ApiService) {}

  getFeatures(): Observable<FeatureListApiResponse> {
    return this.apiService
      .get(ApiUrl.Features.GetByOrganization)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
