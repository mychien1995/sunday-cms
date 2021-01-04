import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { default as Config } from 'app/../assets/config/config.json';

@Injectable()
export class ApiService {
  constructor(private http: HttpClient) {}
  get(path: string, params: HttpParams = new HttpParams()): Observable<any> {
    return this.http.get(`${Config.apiUrl}${path}`, { params });
  }

  put(path: string, body: Object = {}): Observable<any> {
    const headers = this.getHeaders();
    const options = {
      headers: headers,
    };
    return this.http.put(
      `${Config.apiUrl}${path}`,
      JSON.stringify(body),
      options
    );
  }

  postFormData(path, body: FormData): Observable<any> {
    const headers = this.getHeaders(true);
    const options = {
      headers: headers,
    };
    return this.http.post(`${Config.apiUrl}${path}`, body, options);
  }

  post(path: string, body: Object = {}): Observable<any> {
    const headers = this.getHeaders();
    const options = {
      headers: headers,
    };
    return this.http.post(
      `${Config.apiUrl}${path}`,
      JSON.stringify(body),
      options
    );
  }

  delete(path): Observable<any> {
    const headers = this.getHeaders();
    const options = {
      headers: headers,
    };
    return this.http.delete(`${Config.apiUrl}${path}`, options);
  }

  private getHeaders = (isFormDataRequest: boolean = false): HttpHeaders => {
    let headers = new HttpHeaders();
    if (!isFormDataRequest) {
      headers = headers.append(
        'Content-Type',
        'application/json ; charset=utf-8'
      );
    } else {
      //headers = headers.append('Content-Type', 'multipart/form-data');
    }
    headers = headers.append(
      'Accept',
      'application/json , text/javascript, */*; q=0.01'
    );
    headers = headers.append('Access-Control-Allow-Origin', '*');
    return headers;
  };
}
