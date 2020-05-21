import { SharedComponents, SharedResolvers } from './shared.components';
import { SharedServices } from './shared.services';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedAngularMaterial } from './shared.angular-material.module';
import {
  TokenInterceptor,
  AuthenticationInterceptor,
  ErrorInterceptor
} from '@interceptors/index';
import { NgSelectModule } from '@ng-select/ng-select';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    SharedAngularMaterial,
    NgSelectModule,
    ToastrModule.forRoot(),
    BrowserAnimationsModule
  ],
  declarations: [
    SharedComponents
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    SharedAngularMaterial,
    NgSelectModule,
    SharedComponents,
    ToastrModule,
    BrowserAnimationsModule
  ],
  providers: [
    SharedServices,
    SharedResolvers,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    }
  ],
})
export class SharedModule {}
