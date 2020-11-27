import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, Rendering, ListApiResponse } from '@models/index';
import { ApiUrl } from '@core/constants';
import { ApiService } from '@services/api.service';
import { ApiHelper } from '@services/api.helper';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class RenderingService {
  constructor(private apiService: ApiService) {}

  getRenderings(query?: any): Observable<ListApiResponse<Rendering>> {
    return this.apiService
      .post(ApiUrl.Renderings.Search, query)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  create(data: Rendering): Observable<ApiResponse> {
    return this.apiService
      .post(ApiUrl.Renderings.Create, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  update(data: Rendering): Observable<ApiResponse> {
    return this.apiService
      .put(ApiUrl.Renderings.Update, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getById(id: string): Observable<Rendering> {
    return this.apiService
      .get(`${ApiUrl.Renderings.GetById}?id=${id}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  delete(id: string): Observable<ApiResponse> {
    return this.apiService
      .delete(`${ApiUrl.Renderings.Delete}?id=${id}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getRenderingTypes(): Observable<any> {
    return this.apiService
      .get(`${ApiUrl.Renderings.GetRenderingTypes}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
