import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class ClientState {
  private _isBusy: boolean;
  private _isNavigationBusy: boolean;

  public get isBusy(): boolean {
    return this._isBusy;
  }

  public set isBusy(value: boolean) {
    this._isBusy = value;
  }

  public get isNavigationBusy(): boolean {
    return this._isNavigationBusy;
  }

  public set isNavigationBusy(value: boolean) {
    this._isNavigationBusy = value;
  }
}
