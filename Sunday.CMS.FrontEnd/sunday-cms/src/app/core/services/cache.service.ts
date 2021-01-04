import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, share } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Injectable()
export class ResponseCachingService {
  _cache: any = {};
  get(key: string, initiator: () => Observable<any>): Observable<any> {
    if (this._cache[key]) return this._cache[key];
    var data = initiator().pipe(share());
    this._cache[key] = data;
    return data;
  }
}
