import { Injectable } from '@angular/core';
import { ApiHelper } from '@services/api.helper';
import { ApiService } from '@services/api.service';
import { WebsiteList, WebsiteItem, ApiResponse } from '@models/index';
import { ApiUrl } from '@core/constants';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class WebsiteManagementService {
  constructor(private apiService: ApiService) {}

  getWebsites(query?: any): Observable<WebsiteList> {
    return this.apiService
      .post(ApiUrl.Websites.Search, query)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  createWebsite(data: WebsiteItem): Observable<ApiResponse> {
    return this.apiService
      .post(ApiUrl.Websites.Create, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  updateWebsite(data: WebsiteItem): Observable<ApiResponse> {
    return this.apiService
      .put(ApiUrl.Websites.Edit, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getWebsiteById(id: string): Observable<WebsiteItem> {
    return this.apiService
      .get(`${ApiUrl.Websites.GetById}?id=${id}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  deleteWebsite(id: string): Observable<ApiResponse> {
    return this.apiService
      .delete(`${ApiUrl.Websites.Delete}?id=${id}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  activateWebsite(id: string): Observable<ApiResponse> {
    return this.apiService
      .put(`${ApiUrl.Websites.Activate}?id=${id}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
