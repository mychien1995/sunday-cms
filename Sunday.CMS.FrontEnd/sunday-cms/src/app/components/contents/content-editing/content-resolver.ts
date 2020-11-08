import { Injectable, } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { ContentService  } from '@services/index';
import { ContentModel  } from '@models/index';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ContentResolver implements Resolve<ContentModel> {
  constructor(
    private router: Router,
    private contentService: ContentService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {

    return this.contentService.get(route.params['contentId'])
      .pipe(catchError((err) => this.router.navigateByUrl('/')));
  }
}
