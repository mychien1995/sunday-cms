import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ApiUrl } from '@core/constants';
import { ToastrService, Toast } from 'ngx-toastr';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      tap(
        (event) => {},
        (error) => {
          if (error instanceof HttpErrorResponse) {
            if (!request.url.endsWith(ApiUrl.Login) && error.status === 401) {
              if (error.error.message) {
                this.toastr.error(error.error.message);
              } else this.toastr.error('You are not logged in');
              this.router.navigate(['/login']);
            }
          }
        }
      )
    );
  }
}
