import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';

import { WebsiteManagementService } from '@services/index';
import { WebsiteItem } from '@models/index';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AppWebsiteResolver
  implements Resolve<WebsiteItem> {
  constructor(
    private router: Router,
    private service: WebsiteManagementService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {
    return this.service
      .getWebsiteById(route.params['websiteId'])
      .pipe(catchError((err) => this.router.navigateByUrl('/')));
  }
}
