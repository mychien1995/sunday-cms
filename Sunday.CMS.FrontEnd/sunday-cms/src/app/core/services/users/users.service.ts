import { Injectable } from '@angular/core';
import { ApiHelper } from '@services/api.helper';
import { ApiService } from '@services/api.service';
import { UserList, UserMutationModel, CreateUserResponse } from '@models/index';
import { ApiUrl } from '@core/constants';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class UserService {
  constructor(private apiService: ApiService) {}

  getUsers(userQuery?: any): Observable<UserList> {
    return this.apiService
      .post(ApiUrl.Users.Search, userQuery)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  createUser(data: UserMutationModel): Observable<CreateUserResponse> {
    return this.apiService
      .post(ApiUrl.Users.Create, data)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
