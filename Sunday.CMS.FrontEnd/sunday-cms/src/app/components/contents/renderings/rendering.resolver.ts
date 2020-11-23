import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';

import { RenderingService } from '@services/index';
import { Rendering } from '@models/index';
import { catchError } from 'rxjs/operators';

@Injectable()
export class RenderingResolver implements Resolve<Rendering> {
  constructor(private router: Router, private service: RenderingService) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {
    return this.service
      .getById(route.params['renderingId'])
      .pipe(catchError((err) => this.router.navigateByUrl('/')));
  }
}
