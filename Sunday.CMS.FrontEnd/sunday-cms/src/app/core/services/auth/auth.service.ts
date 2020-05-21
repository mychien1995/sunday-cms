import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { _throw } from 'rxjs/observable/throw';
import { LoginInputModel, LoginResponseModel } from '@models/index';
import { JwtHelperService } from '@auth0/angular-jwt';
import { StorageKey } from 'app/core/constants';

@Injectable()
export class AuthenticationService {
  constructor(private jwtService: JwtHelperService) {}

  public isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) {
      return false;
    }
    return !this.jwtService.isTokenExpired(token);
  }

  public storeUserData(loginResponse: LoginResponseModel): void {
    localStorage.removeItem(StorageKey.UserData);
    localStorage.setItem(StorageKey.UserData, JSON.stringify(loginResponse));
  }

  public getToken(): string {
    const storageItem = localStorage.getItem(StorageKey.UserData);
    if (!storageItem) {
      return null;
    }
    const userData = JSON.parse(storageItem);
    return userData.Token;
  }
}
