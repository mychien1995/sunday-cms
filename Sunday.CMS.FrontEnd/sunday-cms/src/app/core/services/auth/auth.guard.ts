import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '@services/auth/auth.service';

@Injectable()

export class AuthGuard implements CanActivate {

  constructor(private authService: AuthenticationService,  private router: Router) {

  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    var isAuthen = this.authService.isAuthenticated();
    if (!isAuthen) {
      this.router.navigate(['/login']);
    }
    return isAuthen;
  }

}
