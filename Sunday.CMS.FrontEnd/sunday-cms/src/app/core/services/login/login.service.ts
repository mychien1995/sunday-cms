import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { _throw } from 'rxjs/observable/throw';
import { LoginInputModel } from '@models/index';
import { ApiHelper } from '@services/api.helper';
import { ApiService } from '@services/api.service';
import { ApiUrl } from '@core/constants';
import { map, catchError } from 'rxjs/operators';


@Injectable()

export class LoginService  {
  constructor(private apiService: ApiService) {

  }

  login(userLogin: LoginInputModel): Observable<any> {
    return this.apiService.post(ApiUrl.Login, userLogin).pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
