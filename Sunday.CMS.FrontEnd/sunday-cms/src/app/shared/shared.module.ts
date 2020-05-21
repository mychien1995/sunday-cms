import {
  LoginComponent,
  DashboardComponent,
  LoadingStateComponent,
  ManageUsersComponent,
  PageHeadingComponent,
  AddUserComponent,
} from 'app/components';
import {
  ApplicationLayoutComponent,
  AppHeaderComponent,
  AppHeaderLogoComponent,
  AppHeaderInfoComponent,
  AppHeaderQuickLinkComponent,
  AppSidebarComponent,
  AppNavigationComponent,
} from 'app/components/_layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import {
  LoginService,
  ApiService,
  AuthenticationService,
  AuthGuard,
  ClientState,
  UserService,
  RoleService
} from '@services/index';
import { SharedAngularMaterial } from './shared.angular-material.module';
import {
  TokenInterceptor,
  AuthenticationInterceptor,
} from '@interceptors/index';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    SharedAngularMaterial,
    NgSelectModule
  ],
  declarations: [
    LoginComponent,
    ApplicationLayoutComponent,
    AppHeaderComponent,
    AppHeaderLogoComponent,
    AppHeaderInfoComponent,
    AppHeaderQuickLinkComponent,
    AppSidebarComponent,
    AppNavigationComponent,
    DashboardComponent,
    LoadingStateComponent,
    ManageUsersComponent,
    PageHeadingComponent,
    AddUserComponent
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    SharedAngularMaterial,
    LoginComponent,
    ApplicationLayoutComponent,
    AppHeaderComponent,
    AppHeaderLogoComponent,
    AppHeaderInfoComponent,
    AppHeaderQuickLinkComponent,
    AppSidebarComponent,
    AppNavigationComponent,
    DashboardComponent,
    LoadingStateComponent,
    ManageUsersComponent,
    PageHeadingComponent,
    AddUserComponent,
    NgSelectModule
  ],
  providers: [
    ApiService,
    LoginService,
    AuthenticationService,
    AuthGuard,
    ClientState,
    UserService,
    RoleService,
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
  ],
})
export class SharedModule {}
