import {NgModule} from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./auth/login/login.component";
import {HeaderComponent} from "./header/header.component";
import {AdminHeaderComponent} from "./admin-header/admin-header.component";
import {MyPatternsComponent} from "./Patterns/my-patterns/my-patterns.component";
import {MyProjectsComponent} from "./Projects/my-projects/my-projects.component";
import {ProjectDetailsComponent} from "./Projects/project-details/project-details.component";
import {CreatePatternComponent} from "./Patterns/create-pattern/create-pattern.component";
import {PatternViewComponent} from "./Patterns/pattern-view/pattern-view.component";
const routes: Routes = [
    {path: 'login', component: LoginComponent},

      {
        path: 'user', component: HeaderComponent, children: [
          {path: 'mypatterns', component: MyPatternsComponent},
          {path: 'myprojects', component: MyProjectsComponent},
          {path: 'projectDetails/:id', component: ProjectDetailsComponent},
          {path:'mypatterns', component: MyPatternsComponent},
          {path: 'createpattern', component: CreatePatternComponent},
          {path: 'viewpattern/:id', component: PatternViewComponent},

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
