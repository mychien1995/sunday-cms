import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { _throw } from 'rxjs/observable/throw';
import { LoginInputModel, LoginResponseModel } from '@models/index';
import { JwtHelperService } from "@auth0/angular-jwt";
import { StorageKey } from 'app/core/constants';


@Injectable()
export class AuthenticationService {
  constructor(private jwtService: JwtHelperService) {

  }

  public isAuthenticated(): boolean {
    const storageItem = localStorage.getItem(StorageKey.UserData);
    if (!storageItem) return false;
    const userData = JSON.parse(storageItem);
    return !this.jwtService.isTokenExpired(userData.Token);
  }

  public storeUserData(loginResponse: LoginResponseModel): void {
    localStorage.removeItem(StorageKey.UserData);
    localStorage.setItem(StorageKey.UserData, JSON.stringify(loginResponse));
  }
}
