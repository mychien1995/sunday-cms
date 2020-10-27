import { HttpErrorResponse, HttpParameterCodec } from '@angular/common/http';
import { _throw } from 'rxjs/observable/throw';

export module ApiHelper {

  export function onSuccess(res: any | any): any {
    return res;
  }

  export function onFail(err: HttpErrorResponse | any) {
    return _throw(err);
  }
}
