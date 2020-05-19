import { LoginComponent } from 'app/components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { LoginService, ApiService } from 'app/core';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [
    LoginComponent
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    LoginComponent
  ],
  providers: [
    ApiService,
    LoginService
  ]
})

export class SharedModule { }
