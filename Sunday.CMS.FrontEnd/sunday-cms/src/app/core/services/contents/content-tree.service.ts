import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ContentTree, ContentTreeList, ContentTreeNode } from '@models/index';
import { ApiUrl } from '@core/constants';
import { ApiService } from '@services/api.service';
import { ApiHelper } from '@services/api.helper';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class ContentTreeService {
  constructor(private apiService: ApiService) {}

  getRoots(): Observable<ContentTree> {
    return this.apiService
      .get(ApiUrl.ContentTree.GetRoots)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }

  getChilds(node: ContentTreeNode): Observable<ContentTreeList> {
    return this.apiService
      .post(ApiUrl.ContentTree.GetChilds, node)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
