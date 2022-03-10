import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './components/about/about.component';
import { LoginComponent } from './components/login/login.component';
import { MainInfoComponent } from './components/main-info/main-info.component';

const routes: Routes = [
  { path: 'about', component: AboutComponent },
  { path: 'login', component: LoginComponent },
  { path: 'main-info', component: MainInfoComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
