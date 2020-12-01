import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, ContentModel, ListApiResponse } from '@models/index';
import { ApiUrl } from '@core/constants';
import { ApiService } from '@services/api.service';
import { ApiHelper } from '@services/api.helper';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class ContentService {
  constructor(private apiService: ApiService) {}

  getMultiples(
    contentIds: string[]
  ): Observable<ListApiResponse<ContentModel>> {
    return this.apiService
      .post(`${ApiUrl.Contents.GetMultiples}`, { ids: contentIds })
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  get(contentId: string, versionId?: string): Observable<ContentModel> {
    return this.apiService
      .get(
        `${ApiUrl.Contents.GetContent}${contentId}?version=${versionId ?? ''}`
      )
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  newVersion(contentId: string, versionId: string): Observable<ApiResponse> {
    return this.apiService
      .post(`${ApiUrl.Contents.NewVersion}${contentId}/${versionId}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  create(node: ContentModel): Observable<ApiResponse> {
    return this.apiService
      .post(ApiUrl.Contents.Create, node)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  update(node: ContentModel): Observable<ApiResponse> {
    return this.apiService
      .post(ApiUrl.Contents.Update, node)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  delete(contentId: string): Observable<ApiResponse> {
    return this.apiService
      .delete(`${ApiUrl.Contents.Delete}${contentId}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  publish(contentId: string): Observable<ApiResponse> {
    return this.apiService
      .put(`${ApiUrl.Contents.Publish}${contentId}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
