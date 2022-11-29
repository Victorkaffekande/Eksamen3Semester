import {NgModule} from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./auth/login/login.component";
import {HeaderComponent} from "./header/header.component";
import {AdminHeaderComponent} from "./admin-header/admin-header.component";
import {MyPatternsComponent} from "./Patterns/my-patterns/my-patterns.component";
import {MyProjectsComponent} from "./Projects/my-projects/my-projects.component";
import {ProjectDetailsComponent} from "./Projects/project-details/project-details.component";


const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {
    path: 'user', component: HeaderComponent, children: [
      {path: 'mypatterns', component: MyPatternsComponent},
      {path: 'myprojects', component: MyProjectsComponent},
      {path: 'projectDetails/:id', component: ProjectDetailsComponent}
    ]
  },

  {path: 'admin', component: AdminHeaderComponent},
  {path: '', component: LoginComponent},
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
