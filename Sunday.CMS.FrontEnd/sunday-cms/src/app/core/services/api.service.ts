import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Injectable()
export class ApiService {
  constructor(
    private http: HttpClient
  ) { }
  get(path: string, params: HttpParams = new HttpParams()): Observable<any> {
    return this.http.get(`${environment.apiUrl}${path}`, { params });
  }

  put(path: string, body: Object = {}): Observable<any> {
    let headers = this.getHeaders();
    let options = {
      headers: headers
    };
    return this.http.put(
      `${environment.apiUrl}${path}`,
      JSON.stringify(body),
      options
    );
  }

  post(path: string, body: Object = {}): Observable<any> {
    let headers = this.getHeaders();
    let options = {
      headers: headers
    };
    return this.http.post(
      `${environment.apiUrl}${path}`,
      JSON.stringify(body),
      options
    );
  }

  delete(path): Observable<any> {
    return this.http.delete(
      `${environment.apiUrl}${path}`
    );
  }

  private getHeaders = (isFormDataRequest: boolean = false): HttpHeaders => {
    let headers = new HttpHeaders();
    if (!isFormDataRequest) headers = headers.append('Content-Type', 'application/json ; charset=utf-8');
    headers = headers.append('Accept', 'application/json , text/javascript, */*; q=0.01');
    headers = headers.append('Access-Control-Allow-Origin', '*');
    return headers;
  }
}
