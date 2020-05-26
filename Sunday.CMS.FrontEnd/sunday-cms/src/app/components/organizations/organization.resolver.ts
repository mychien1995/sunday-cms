import { Injectable, } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { OrganizationService } from '@services/index';
import { OrganizationDetailResponse } from '@models/index';
import { catchError } from 'rxjs/operators';

@Injectable()
export class OrganizationResolver implements Resolve<OrganizationDetailResponse> {
  constructor(
    private router: Router,
    private organizationService: OrganizationService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {

    return this.organizationService.getOrganizationById(route.params['orgId'])
      .pipe(catchError((err) => this.router.navigateByUrl('/')));
  }
}
