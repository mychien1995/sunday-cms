import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';

import { LayoutManagementService } from '@services/index';
import { LayoutItem } from '@models/index';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AppLayoutResolver
  implements Resolve<LayoutItem> {
  constructor(
    private router: Router,
    private service: LayoutManagementService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {
    return this.service
      .getLayoutById(route.params['layoutId'])
      .pipe(catchError((err) => this.router.navigateByUrl('/')));
  }
}
