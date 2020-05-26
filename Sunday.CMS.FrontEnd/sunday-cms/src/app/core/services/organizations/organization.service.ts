import { Injectable } from '@angular/core';
import { ApiHelper } from '@services/api.helper';
import { ApiService } from '@services/api.service';
import {
  OrganizationList,
  OrganizationMutationModel,
  ApiResponse,
  OrganizationDetailResponse,
} from '@models/index';
import { ApiUrl } from '@core/constants';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class OrganizationService {
  constructor(private apiService: ApiService) {}

  getOrganizations(organizationQuery?: any): Observable<OrganizationList> {
    return this.apiService
      .post(ApiUrl.Organizations.Search, organizationQuery)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  createOrganization(data: OrganizationMutationModel): Observable<ApiResponse> {
    return this.apiService
      .post(ApiUrl.Organizations.Create, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  updateOrganization(data: OrganizationMutationModel): Observable<ApiResponse> {
    return this.apiService
      .put(ApiUrl.Organizations.Edit, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getOrganizationById(orgId: number): Observable<OrganizationDetailResponse> {
    return this.apiService
      .get(`${ApiUrl.Organizations.GetById}?id=${orgId}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
  activateOrganization(userId: number): Observable<ApiResponse> {
    return this.apiService
      .put(`${ApiUrl.Organizations.Activate}?id=${userId}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  deactivateOrganization(userId: number): Observable<ApiResponse> {
    return this.apiService
      .put(`${ApiUrl.Organizations.Deactivate}?id=${userId}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
