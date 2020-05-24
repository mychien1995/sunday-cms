import { RoleModel } from '@models/index';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiHelper } from '@services/api.helper';
import { ApiService } from '@services/api.service';
import { ApiUrl } from '@core/constants';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class RoleService {
  constructor(private apiService: ApiService) {}

  getAvailableRoles(): Observable<any> {
    return this.apiService
      .get(ApiUrl.Roles.GetAll)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getRoleById(id: number): Observable<RoleModel> {
    return this.apiService
      .get(ApiUrl.Roles.GetById + id)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
