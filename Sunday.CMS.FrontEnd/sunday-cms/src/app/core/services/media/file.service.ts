import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiHelper } from '@services/api.helper';
import { ApiService } from '@services/api.service';
import { ApiUrl } from '@core/constants';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class FileService {
  constructor(private apiService: ApiService) {}

  previewImage(blobUri: string): Observable<any> {
    return this.apiService
      .get(`${ApiUrl.FilePreview}?id=${blobUri}`)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
