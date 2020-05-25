import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiHelper } from '@services/api.helper';
import { ApiService } from '@services/api.service';
import { ApiUrl } from '@core/constants';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class FileUploadService {
  constructor(private apiService: ApiService) {}

  uploadBlob(directory: string, file: File): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('FileUpload', file, file.name);
    formData.append('Directory', directory);
    return this.apiService
      .postFormData(ApiUrl.FileUpload, formData)
      .pipe(map(ApiHelper.onSuccess), catchError(ApiHelper.onFail));
  }
}
