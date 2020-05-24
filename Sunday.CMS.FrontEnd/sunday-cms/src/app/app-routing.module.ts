import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {
  LoginComponent,
  ApplicationLayoutComponent,
  DashboardComponent,
  ManageUsersComponent,
  AddUserComponent,
  UserProfileComponent
} from 'app/components';
import { AuthGuard } from 'app/core/services';
import { UserResolver } from '@components/index';

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
        resolve: { user: UserResolver }
      },
      { path: 'profile', component: UserProfileComponent }
    ],
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
