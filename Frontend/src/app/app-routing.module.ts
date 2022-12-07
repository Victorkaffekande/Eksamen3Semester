import {NgModule} from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./auth/login/login.component";
import {HeaderComponent} from "./header/header.component";
import {AdminHeaderComponent} from "./admin/admin-header/admin-header.component";
import {MyPatternsComponent} from "./Patterns/my-patterns/my-patterns.component";
import {MyProjectsComponent} from "./Projects/my-projects/my-projects.component";
import {ProjectDetailsComponent} from "./Projects/project-details/project-details.component";
import {CreatePatternComponent} from "./Patterns/create-pattern/create-pattern.component";
import {PatternViewComponent} from "./Patterns/pattern-view/pattern-view.component";
import {DiscoverComponent} from "./DiscoverSite/discover/discover.component";
import {PublicUserProfileComponent} from "./UserProfiles/public-user-profile/public-user-profile.component";
import {PrivateUserProfileComponent} from "./UserProfiles/private-user-profile/private-user-profile.component";
import {AdminPatternsComponent} from "./admin/admin-patterns/admin-patterns.component";
import {AdminUsersComponent} from "./admin/admin-users/admin-users.component";
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
          {path: 'discover', component: DiscoverComponent},
          {path: 'userprofile/:id', component: PublicUserProfileComponent},
        ]
      },
  {path: 'admin', component: AdminHeaderComponent, children: [
      {path: 'patterns', component: AdminPatternsComponent},
      {path: 'users', component: AdminUsersComponent},
    ]},
  {path: '', component: LoginComponent},
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
