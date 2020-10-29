import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {
  LoginComponent,
  ApplicationLayoutComponent,
  DashboardComponent,
  ManageUsersComponent,
  AddUserComponent,
  UserProfileComponent,
  ManageOrganizationComponent,
  AddOrganizationComponent,
  ManageOrganizationRolesComponent,
  AddOrganizationRoleComponent,
  ManagePermissionsComponent,
  ManageOrganizationUsersComponent,
  AddOrganizationUserComponent,
  ManageLayoutComponent,
  AddLayoutComponent,
  AddWebsiteComponent,
  ManageWebsiteComponent,
} from 'app/components';
import { AuthGuard } from 'app/core/services';
import {
  UserResolver,
  OrganizationResolver,
  OrganizationRoleResolver,
  AppLayoutResolver,
  AppWebsiteResolver,
} from '@components/index';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: ApplicationLayoutComponent,
    children: [
      { path: '', component: DashboardComponent, pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, pathMatch: 'full' },
      { path: 'users', component: ManageUsersComponent },
      { path: 'users/create', component: AddUserComponent },
      {
        path: 'users/edit/:userId',
        component: AddUserComponent,
        resolve: { user: UserResolver },
      },
      { path: 'profile', component: UserProfileComponent },
      { path: 'organizations', component: ManageOrganizationComponent },
      { path: 'organizations/create', component: AddOrganizationComponent },
      {
        path: 'organizations/edit/:orgId',
        component: AddOrganizationComponent,
        resolve: { organization: OrganizationResolver },
      },
      {
        path: 'organization-roles',
        component: ManageOrganizationRolesComponent,
      },
      {
        path: 'organization-roles/create',
        component: AddOrganizationRoleComponent,
      },
      {
        path: 'organization-roles/edit/:roleId',
        component: AddOrganizationRoleComponent,
        resolve: { role: OrganizationRoleResolver },
      },
      {
        path: 'permissions-manager',
        component: ManagePermissionsComponent,
      },
      {
        path: 'organization-users',
        component: ManageOrganizationUsersComponent,
      },
      {
        path: 'organization-users/create',
        component: AddOrganizationUserComponent,
      },
      {
        path: 'organization-users/edit/:userId',
        component: AddOrganizationUserComponent,
        resolve: { user: UserResolver },
      },
      {
        path: 'manage-layouts',
        component: ManageLayoutComponent,
      },
      {
        path: 'manage-layouts/create',
        component: AddLayoutComponent,
      },
      {
        path: 'manage-layouts/edit/:layoutId',
        component: AddLayoutComponent,
        resolve: { layout: AppLayoutResolver },
      },
      {
        path: 'manage-websites',
        component: ManageWebsiteComponent,
      },
      {
        path: 'manage-websites/create',
        component: AddWebsiteComponent,
      },
      {
        path: 'manage-websites/edit/:websiteId',
        component: AddWebsiteComponent,
        resolve: { website: AppWebsiteResolver },
      },
    ],
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
