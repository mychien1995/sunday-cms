import { LoginComponent, DashboardComponent, LoadingStateComponent, ManageUsersComponent } from 'app/components';
import {
  ApplicationLayoutComponent, AppHeaderComponent,
  AppHeaderLogoComponent, AppHeaderInfoComponent, AppHeaderQuickLinkComponent,
  AppSidebarComponent, AppNavigationComponent
} from 'app/components/_layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginService, ApiService, AuthenticationService, AuthGuard, ClientState, UserService } from '@services/index';
import { SharedAngularMaterial } from './shared.angular-material.module';

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
    ManageUsersComponent
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
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
    SharedAngularMaterial
  ],
  providers: [
    ApiService,
    LoginService,
    AuthenticationService,
    AuthGuard,
    ClientState,
    UserService
  ]
})

export class SharedModule { }
