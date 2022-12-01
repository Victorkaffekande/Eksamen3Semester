import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { LoginComponent } from './auth/login/login.component';
import { AppRoutingModule } from './app-routing.module';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { HeaderComponent } from './header/header.component';
import { AdminHeaderComponent } from './admin-header/admin-header.component';
import { RegisterComponent } from './auth/register/register.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MyPatternsComponent } from './Patterns/my-patterns/my-patterns.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatStepperModule} from "@angular/material/stepper";
import {MatFormFieldModule} from "@angular/material/form-field";
import { MyProjectsComponent } from './Projects/my-projects/my-projects.component';
import { CreateProjectComponent } from './Projects/create-project/create-project.component';
import { ProjectDetailsComponent } from './Projects/project-details/project-details.component';
import { CreatePatternComponent } from './Patterns/create-pattern/create-pattern.component';
import {MatButtonModule} from "@angular/material/button";
import {MatInputModule} from "@angular/material/input";
import {CdkStepperModule} from "@angular/cdk/stepper";
import { PatternViewComponent } from './Patterns/pattern-view/pattern-view.component';
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import { DiscoverComponent } from './DiscoverSite/discover/discover.component';
import { Custom_Ng2SearchPipe } from './DiscoverSite/discover/custom-search.pipe';



@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HeaderComponent,
    AdminHeaderComponent,
    RegisterComponent,
    MyPatternsComponent,
    MyProjectsComponent,
    CreateProjectComponent,
    ProjectDetailsComponent,
    CreatePatternComponent,
    PatternViewComponent,
    DiscoverComponent,
    Custom_Ng2SearchPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    BrowserAnimationsModule,
    MatStepperModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    CdkStepperModule,
    MatProgressSpinnerModule,

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
