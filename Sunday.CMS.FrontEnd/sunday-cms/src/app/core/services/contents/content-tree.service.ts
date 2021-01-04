import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import {
  ContentTree,
  ContentTreeList,
  ContentTreeNode,
  ContextMenu,
} from '@models/index';
import { ApiUrl } from '@core/constants';
import { ApiService } from '@services/api.service';
import { ApiHelper } from '@services/api.helper';
import { map, catchError } from 'rxjs/operators';
import { ResponseCachingService } from '@services/cache.service';

@Injectable()
export class ContentTreeService {
  constructor(
    private apiService: ApiService,
    private cacheService: ResponseCachingService
  ) {}

  getRoots(): Observable<ContentTree> {
    return this.apiService
      .get(ApiUrl.ContentTree.GetRoots)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getChilds(node: ContentTreeNode): Observable<ContentTreeList> {
    return this.apiService
      .post(ApiUrl.ContentTree.GetChilds, this.withoutReference(node))
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getContextMenu(node: ContentTreeNode): Observable<ContextMenu> {
    return this.apiService
      .post(ApiUrl.ContentTree.GetContextMenu, this.withoutReference(node))
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getTreeByPath(path: string): Observable<ContentTree> {
    return this.apiService
      .get(`${ApiUrl.ContentTree.GetByPath}?path=${path}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getTreeByQuery(query: any): Observable<ContentTree> {
    return this.cacheService.get(
      `getTreeByQuery_${JSON.stringify(query)}`,
      () => {
        return this.apiService
          .post(`${ApiUrl.ContentTree.GetByQuery}`, query)
          .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
      }
    );
  }

  withoutReference(node: ContentTreeNode) {
    const clone = { ...node };
    clone.ParentNode = null;
    clone.ChildNodes = [];
    return clone;
  }
}
