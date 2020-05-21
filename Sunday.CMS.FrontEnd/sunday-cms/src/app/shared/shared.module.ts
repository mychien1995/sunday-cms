import { LoginComponent, DashboardComponent, LoadingStateComponent, ManageUsersComponent, PageHeadingComponent } from 'app/components';
import {
  ApplicationLayoutComponent, AppHeaderComponent,
  AppHeaderLogoComponent, AppHeaderInfoComponent, AppHeaderQuickLinkComponent,
  AppSidebarComponent, AppNavigationComponent
} from 'app/components/_layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginService, ApiService, AuthenticationService, AuthGuard, ClientState, UserService } from '@services/index';
import { SharedAngularMaterial } from './shared.angular-material.module';
import { TokenInterceptor, AuthenticationInterceptor } from '@interceptors/index';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    SharedAngularMaterial
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
    PageHeadingComponent
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
    PageHeadingComponent
  ],
  providers: [
    ApiService,
    LoginService,
    AuthenticationService,
    AuthGuard,
    ClientState,
    UserService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    }
  ]
})

export class SharedModule { }
