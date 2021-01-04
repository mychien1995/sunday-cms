import { Injectable, Output, EventEmitter } from '@angular/core';
import { Observable, forkJoin } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ApiUrl } from '@core/constants';
import { ApiService } from '@services/api.service';
import { ApiHelper } from '@services/api.helper';
import { map, catchError } from 'rxjs/operators';
import { ApiResponse, LayoutModel, NavigationTree } from '@models/index';
import { StorageKey } from 'app/core/constants';
import { ClientState } from './clientstate.service';
import { ResponseCachingService } from '@services/cache.service';

@Injectable()
export class LayoutService {
  @Output() layoutBus: EventEmitter<any> = new EventEmitter();

  constructor(
    private apiService: ApiService,
    private clientState: ClientState,
    private cacheService: ResponseCachingService
  ) {}

  layoutUpdated(data: any): void {
    this.layoutBus.emit(data);
  }

  clearCache() {
    localStorage.removeItem(StorageKey.NavigationData);
    localStorage.removeItem(StorageKey.LayoutData);
  }

  private getInternalData<T>(cacheKey: string): T {
    const storageItem = localStorage.getItem(cacheKey);
    if (!storageItem) {
      this.refresh();
      return null;
    }
    const data = JSON.parse(storageItem);
    const now = new Date().getTime();
    if (now - data.CreatedDate > 300000) {
      this.refresh();
      return null;
    }
    return <T>data;
  }

  getNavigation(): NavigationTree {
    return this.getInternalData<NavigationTree>(StorageKey.NavigationData);
  }

  getLayout(): LayoutModel {
    return this.getInternalData<LayoutModel>(StorageKey.LayoutData);
  }

  refresh(callback?: any): void {
    this.clearCache();
    this.clientState.isNavigationBusy = true;
    forkJoin([this.getNavigationData(), this.getLayoutData()]).subscribe(
      (response) => {
        if (response[0].Success) {
          this.saveInternalData<NavigationTree>(
            <NavigationTree>response[0],
            StorageKey.NavigationData
          );
          this.layoutUpdated({
            event: 'navigation-updated',
          });
        }
        if (response[1].Success) {
          this.saveInternalData<LayoutModel>(
            <LayoutModel>response[1],
            StorageKey.LayoutData
          );
          this.layoutUpdated({
            event: 'layout-updated',
          });
        }
        this.clientState.isNavigationBusy = false;
        if (callback) {
          callback();
        }
      }
    );
  }

  private saveInternalData<T>(data: T, key: string): void {
    data['CreatedDate'] = new Date().getTime();
    localStorage.setItem(key, JSON.stringify(data));
  }

  private getNavigationData(): Observable<ApiResponse> {
    return this.cacheService.get(`getNavigationData`, () => {
      return this.apiService
        .get(ApiUrl.Layout.GetNavigation)
        .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
    });
  }

  private getLayoutData(): Observable<ApiResponse> {
    return this.cacheService.get(`getLayoutData`, () => {
      return this.apiService
        .get(ApiUrl.Layout.GetLayout)
        .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
    });
  }
}
