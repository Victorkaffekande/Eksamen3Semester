import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./auth/login/login.component";
import {HeaderComponent} from "./header/header.component";
import {AdminHeaderComponent} from "./admin-header/admin-header.component";
import * as path from "path";
import {MyPatternsComponent} from "./Patterns/my-patterns/my-patterns.component";
import {CreatePatternComponent} from "./Patterns/create-pattern/create-pattern.component";


const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'user', component: HeaderComponent, children:[
      {
        path:'mypatterns', component: MyPatternsComponent
      },
      {path: 'createpattern', component: CreatePatternComponent},

    ]},


  {path: 'admin', component: AdminHeaderComponent},
  {path: '', component: LoginComponent},
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
