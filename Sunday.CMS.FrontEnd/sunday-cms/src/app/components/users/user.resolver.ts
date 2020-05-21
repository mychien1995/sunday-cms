import { Injectable, } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { UserService } from '@services/index';
import { UserDetailResponse } from '@models/index';
import { catchError } from 'rxjs/operators';

@Injectable()
export class UserResolver implements Resolve<UserDetailResponse> {
  constructor(
    private router: Router,
    private userService: UserService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {

    return this.userService.getUserById(route.params['userId'])
      .pipe(catchError((err) => this.router.navigateByUrl('/')));
  }
}
