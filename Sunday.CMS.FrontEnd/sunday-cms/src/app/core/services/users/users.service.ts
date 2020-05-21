import { Injectable } from '@angular/core';
import { ApiService, ApiHelper } from '@services/index';
import { UserList, UserItem } from '@models/index';
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
}
