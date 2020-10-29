import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';

import { TemplateManagementService } from '@services/index';
import { TemplateItem } from '@models/index';
import { catchError } from 'rxjs/operators';

@Injectable()
export class TemplateResolver implements Resolve<TemplateItem> {
  constructor(
    private router: Router,
    private service: TemplateManagementService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {
    return this.service
      .getTemplateById(route.params['templateId'])
      .pipe(catchError((err) => this.router.navigateByUrl('/')));
  }
}
