import { HttpErrorResponse, HttpParameterCodec } from '@angular/common/http';
import { _throw } from 'rxjs/observable/throw';

export module ApiHelper {
  export function onSuccess(res: any | any): any {
    let body = res;
    return body;
  }

  export function onFail(err: HttpErrorResponse | any) {
    return _throw(err);
  }
}
