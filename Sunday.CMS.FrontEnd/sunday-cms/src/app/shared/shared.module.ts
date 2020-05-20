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
import { LoginService, ApiService, AuthenticationService, AuthGuard, ClientState } from '@services/index';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterModule
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
    ManageUsersComponent
  ],
  providers: [
    ApiService,
    LoginService,
    AuthenticationService,
    AuthGuard,
    ClientState
  ]
})

export class SharedModule { }
