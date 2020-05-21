import {
  LoginComponent,
  DashboardComponent,
  LoadingStateComponent,
  ManageUsersComponent,
  PageHeadingComponent,
  AddUserComponent
} from 'app/components';
import {
  ApplicationLayoutComponent,
  AppHeaderComponent,
  AppHeaderLogoComponent,
  AppHeaderInfoComponent,
  AppHeaderQuickLinkComponent,
  AppSidebarComponent,
  AppNavigationComponent
} from 'app/components/_layout';
import { UserResolver } from 'app/components';
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
  AppNavigationComponent
];
export const SharedResolvers = [
    UserResolver
]