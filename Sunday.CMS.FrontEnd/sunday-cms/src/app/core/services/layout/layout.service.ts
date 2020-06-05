import { Injectable, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ApiUrl } from '@core/constants';
import { ApiService } from '@services/api.service';
import { ApiHelper } from '@services/api.helper';
import { map, catchError } from 'rxjs/operators';
import { ApiResponse, NavigationTree } from '@models/index';
import { StorageKey } from 'app/core/constants';
import { ClientState } from './clientstate.service';

@Injectable()
export class LayoutService {
  @Output() layoutBus: EventEmitter<any> = new EventEmitter();

  constructor(
    private apiService: ApiService,
    private clientState: ClientState
  ) {}

  layoutUpdated(data: any): void {
    this.layoutBus.emit(data);
  }

  clearNavigationCache() {
    localStorage.removeItem(StorageKey.NavigationData);
  }

  getNavigation(): NavigationTree {
    const storageItem = localStorage.getItem(StorageKey.NavigationData);
    if (!storageItem) {
      this.refreshNavigation();
      return null;
    }
    const navigationData = JSON.parse(storageItem);
    const now = new Date().getTime();
    if (now - navigationData.CreatedDate > 300000) {
      this.refreshNavigation();
      return null;
    }
    return <NavigationTree>navigationData;
  }

  refreshNavigation(callback?: any): void {
    this.clearNavigationCache();
    this.clientState.isNavigationBusy = true;
    this.getNavigationData().subscribe((res) => {
      if (res.Success) {
        this.saveNavigation(<NavigationTree>res);
        this.layoutUpdated({
          event: 'navigation-updated',
        });
        this.clientState.isNavigationBusy = false;
        if (callback) {
          callback();
        }
      }
    });
  }

  private saveNavigation(data: NavigationTree): void {
    this.clearNavigationCache();
    data.CreatedDate = new Date().getTime();
    const navigationStr = JSON.stringify(data);
    localStorage.setItem(StorageKey.NavigationData, navigationStr);
  }

  getNavigationData(): Observable<ApiResponse> {
    return this.apiService
      .get(ApiUrl.Layout.GetNavigation)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
