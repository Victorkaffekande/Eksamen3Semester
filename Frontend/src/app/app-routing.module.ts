import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./login/login.component";
import {HeaderComponent} from "./header/header.component";
import {AdminHeaderComponent} from "./admin-header/admin-header.component";


const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'user', component: HeaderComponent},
  {path: 'admin', component: AdminHeaderComponent},
  {path: '', component: LoginComponent},
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
