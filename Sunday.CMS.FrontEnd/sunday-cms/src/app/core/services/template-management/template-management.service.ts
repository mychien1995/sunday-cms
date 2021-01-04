import { Injectable } from '@angular/core';
import { ApiHelper } from '@services/api.helper';
import { ApiService } from '@services/api.service';
import {
  TemplateList,
  TemplateItem,
  ApiResponse,
  GenericApiResponse,
  FieldTypeList,
  TemplateField,
} from '@models/index';
import { ApiUrl } from '@core/constants';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ResponseCachingService } from '@services/cache.service';

@Injectable()
export class TemplateManagementService {
  constructor(
    private apiService: ApiService,
    private cachingService: ResponseCachingService
  ) {}

  getTemplates(query?: any): Observable<TemplateList> {
    return this.cachingService.get(
      `getTemplates_${JSON.stringify(query)}`,
      () => {
        return this.apiService
          .post(ApiUrl.Templates.Search, query)
          .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
      }
    );
  }

  createTemplate(data: TemplateItem): Observable<ApiResponse> {
    return this.apiService
      .post(ApiUrl.Templates.Create, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  updateTemplate(data: TemplateItem): Observable<ApiResponse> {
    return this.apiService
      .put(ApiUrl.Templates.Edit, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getTemplateById(id: string): Observable<TemplateItem> {
    return this.apiService
      .get(`${ApiUrl.Templates.GetById}?id=${id}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  deleteTemplate(id: string): Observable<ApiResponse> {
    return this.apiService
      .delete(`${ApiUrl.Templates.Delete}?id=${id}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getFieldTypes(): Observable<FieldTypeList> {
    return this.apiService
      .get(ApiUrl.Templates.GetFieldTypes)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getFields(
    templateId: string
  ): Observable<GenericApiResponse<TemplateField[]>> {
    return this.apiService
      .get(`${ApiUrl.Templates.LoadFields}?id=${templateId}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
