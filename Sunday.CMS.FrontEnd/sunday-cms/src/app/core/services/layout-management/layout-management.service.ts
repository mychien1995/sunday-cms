import { Injectable } from '@angular/core';
import { ApiHelper } from '@services/api.helper';
import { ApiService } from '@services/api.service';
import { LayoutList, LayoutItem, ApiResponse } from '@models/index';
import { ApiUrl } from '@core/constants';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class LayoutManagementService {
  constructor(private apiService: ApiService) {}

  getLayouts(query?: any): Observable<LayoutList> {
    return this.apiService
      .post(ApiUrl.Layouts.Search, query)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  createLayouts(data: LayoutItem): Observable<ApiResponse> {
    return this.apiService
      .post(ApiUrl.Layouts.Create, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  updateLayout(data: LayoutItem): Observable<ApiResponse> {
    return this.apiService
      .put(ApiUrl.Layouts.Edit, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getLayoutById(id: string): Observable<LayoutItem> {
    return this.apiService
      .get(`${ApiUrl.Layouts.GetById}?id=${id}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  deleteLayout(id: string): Observable<ApiResponse> {
    return this.apiService
      .delete(`${ApiUrl.Layouts.Delete}?id=${id}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
