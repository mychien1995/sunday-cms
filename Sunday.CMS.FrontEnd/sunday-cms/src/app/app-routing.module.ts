import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent, ApplicationLayoutComponent, DashboardComponent } from 'app/components';
import { AuthGuard } from 'app/core/services';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '', component: ApplicationLayoutComponent,
    children: [
      { path: '', component: DashboardComponent, pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, pathMatch: 'full' }
    ],
    canActivate: [AuthGuard]
  }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
