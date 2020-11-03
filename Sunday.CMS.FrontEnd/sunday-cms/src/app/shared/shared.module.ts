import { SharedComponents, SharedResolvers } from './shared.components';
import { SharedServices } from './shared.services';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedAngularMaterial } from './shared.angular-material.module';
import { SharedBootstraps } from './shared.bootstrap-module';
import { SharedPipes } from './shared.pipes';
import {
  TokenInterceptor,
  AuthenticationInterceptor,
  ErrorInterceptor,
} from '@interceptors/index';
import { NgSelectModule } from '@ng-select/ng-select';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ContentComponents } from './content.component';
import { ContentServices } from './content.services';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    SharedAngularMaterial,
    SharedBootstraps,
    NgSelectModule,
    ToastrModule.forRoot(),
    BrowserAnimationsModule,
    NgMultiSelectDropDownModule.forRoot(),
  ],
  declarations: [SharedComponents, SharedPipes, ContentComponents],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    SharedAngularMaterial,
    SharedBootstraps,
    NgSelectModule,
    SharedComponents,
    ContentComponents,
    ToastrModule,
    SharedPipes,
    BrowserAnimationsModule,
  ],
  providers: [
    SharedServices,
    ContentServices,
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
    },
  ],
})
export class SharedModule {}
