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
import { CreatePatternComponent } from './Patterns/create-pattern/create-pattern.component';
import {MatButtonModule} from "@angular/material/button";
import {MatInputModule} from "@angular/material/input";
import {CdkStepperModule} from "@angular/cdk/stepper";
import { PatternViewComponent } from './Patterns/pattern-view/pattern-view.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HeaderComponent,
    AdminHeaderComponent,
    RegisterComponent,
    MyPatternsComponent,
    CreatePatternComponent,
    PatternViewComponent,
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
    CdkStepperModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
