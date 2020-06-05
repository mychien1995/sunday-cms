import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import {
  OrganizationRoleListApiResponse,
  OrganizationRoleMutationData,
  OrganizationRoleDetailApiResponse,
  ApiResponse,
} from '@models/index';
import { ApiUrl } from '@core/constants';
import { ApiService } from '@services/api.service';
import { ApiHelper } from '@services/api.helper';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class OrganizationRoleService {
  constructor(private apiService: ApiService) {}

  getRoles(query?: any): Observable<OrganizationRoleListApiResponse> {
    return this.apiService
      .post(ApiUrl.OrganizationRoles.Search, query)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getRoleById(roleId: number): Observable<OrganizationRoleDetailApiResponse> {
    return this.apiService
      .get(`${ApiUrl.OrganizationRoles.GetById}?id=${roleId}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  createRole(data: OrganizationRoleMutationData): Observable<ApiResponse> {
    return this.apiService
      .post(ApiUrl.OrganizationRoles.Create, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  updateRole(data: OrganizationRoleMutationData): Observable<ApiResponse> {
    return this.apiService
      .put(ApiUrl.OrganizationRoles.Update, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  deleteRole(roleId: number): Observable<ApiResponse> {
    return this.apiService
      .delete(`${ApiUrl.OrganizationRoles.Delete}?id=${roleId}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  bulkUpdate(roles: OrganizationRoleMutationData[]): Observable<ApiResponse> {
    return this.apiService
      .post(`${ApiUrl.OrganizationRoles.BulkUpdate}`, { Roles: roles })
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
