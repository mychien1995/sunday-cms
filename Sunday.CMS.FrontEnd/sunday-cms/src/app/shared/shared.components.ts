import {
  LoginComponent,
  DashboardComponent,
  LoadingStateComponent,
  ManageUsersComponent,
  PageHeadingComponent,
  AddUserComponent,
  UserProfileComponent,
  ChangePasswordDialogComponent,
  UserFilterComponent,
  ChangeAvatarDialogComponent,
  ManageOrganizationComponent,
  AddOrganizationComponent,
  OrganizationFormComponent,
  OrganizationFilterComponent,
  OrganizationAssignBoardComponent
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
import { UserResolver, OrganizationResolver } from 'app/components';
export const SharedComponents = [
  LoginComponent,
  DashboardComponent,
  LoadingStateComponent,
  ManageUsersComponent,
  PageHeadingComponent,
  AddUserComponent,
  ApplicationLayoutComponent,
  AppHeaderComponent,
  AppHeaderLogoComponent,
  AppHeaderInfoComponent,
  AppHeaderQuickLinkComponent,
  AppSidebarComponent,
  AppNavigationComponent,
  UserProfileComponent,
  ChangePasswordDialogComponent,
  UserFilterComponent,
  ChangeAvatarDialogComponent,
  ManageOrganizationComponent,
  AddOrganizationComponent,
  OrganizationFormComponent,
  OrganizationFilterComponent,
  OrganizationAssignBoardComponent
];
export const SharedResolvers = [UserResolver, OrganizationResolver];
