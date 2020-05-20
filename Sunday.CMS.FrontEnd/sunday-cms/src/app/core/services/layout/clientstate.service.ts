import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()

export class ClientState {
  private _isBusy: boolean;

  public get isBusy(): boolean {
    return this._isBusy;
  }

  public set isBusy(value: boolean) {
    this._isBusy = value;
  }
}
