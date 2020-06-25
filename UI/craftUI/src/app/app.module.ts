import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes, Route } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormComponent } from './components/form/form.component';
import { RestService } from './services/rest.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppConfig } from './app.config';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatInputModule } from "@angular/material/input";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatCardModule } from "@angular/material/card";
import { LoginComponent } from './components/login/login.component';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { CalculationService } from './services/calculation.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { StorageService } from './services/storage.service';
import { CreateAccountComponent } from './components/account/create-account.component';
import { IAMService } from './services/iam.service';
import { UserService } from './services/user.service';
import { AuthGuard } from './guards/auth.guard';
import { CurrencyService } from './services/currency.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SpinnerService } from './services/spinner.service';

@NgModule({
  declarations: [
    AppComponent,
    FormComponent,
    LoginComponent,
    CreateAccountComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSidenavModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    RouterModule.forRoot([
        { path: '', component: LoginComponent },
        { path: 'form', component: FormComponent, canActivate: [AuthGuard] },
        { path: 'create-acct', component: CreateAccountComponent},
        { path: '', redirectTo: '', pathMatch: 'full' },
        { path: '**', redirectTo: '' }]),
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    HttpClientModule,
    MatSnackBarModule
  ],
  providers: [
    RestService,
    CalculationService,
    HttpClient,
    StorageService,
    IAMService,
    UserService,
    CurrencyService,
    AppConfig,
    AuthGuard,
    SpinnerService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
