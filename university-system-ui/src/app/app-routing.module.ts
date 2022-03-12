import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { AboutComponent } from './components/about/about.component';
import { AdminComponent } from './components/admin/admin.component';
import { LoginComponent } from './components/login/login.component';
import { MainInfoComponent } from './components/main-info/main-info.component';

const routes: Routes = [
  { path: 'about', component: AboutComponent },
  { path: 'login', component: LoginComponent },
  { path: 'main-info', component: MainInfoComponent },
  { path: 'admin', component: AdminComponent,
  canActivate: [AuthGuard],
  data: {
    role: 'admin'
  }
 }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
