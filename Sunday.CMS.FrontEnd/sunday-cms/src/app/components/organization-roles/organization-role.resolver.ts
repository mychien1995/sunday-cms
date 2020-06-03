import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';

import { OrganizationRoleService } from '@services/index';
import { OrganizationRoleDetailApiResponse } from '@models/index';
import { catchError } from 'rxjs/operators';

@Injectable()
export class OrganizationRoleResolver
  implements Resolve<OrganizationRoleDetailApiResponse> {
  constructor(
    private router: Router,
    private roleService: OrganizationRoleService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {
    return this.roleService
      .getRoleById(route.params['roleId'])
      .pipe(catchError((err) => this.router.navigateByUrl('/')));
  }
}
