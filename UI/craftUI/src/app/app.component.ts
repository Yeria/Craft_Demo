import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { FormGroup, FormControl, FormGroupDirective, NgForm, Validators } from "@angular/forms";
import { ErrorStateMatcher } from "@angular/material/core";
import { StorageService } from './services/storage.service';
import { SpinnerService } from './services/spinner.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'craftUI';
  text = "Contact Page";
  contactForm: FormGroup;
  contact = {
      email: "",
      password: ""
  };
  submitted = false;
  navToggleState = false;
  matcher = new MyErrorStateMatcher();
  spinner: SpinnerService;

  constructor(public readonly router: Router,
    public readonly storageService: StorageService,
    private readonly spinnerService: SpinnerService){
    this.createForm();
    this.spinner = spinnerService;
  }

  goHome(): void {
    this.router.navigate(["/"]);
  }

  signOut(): void {
    this.storageService.removeSessionItem("token");
    this.goHome();
  }

  createForm(): void {
      this.contactForm = new FormGroup({
          "email": new FormControl(this.contact.email, [Validators.required, Validators.email]),
          "password": new FormControl(this.contact.password, [Validators.required, Validators.minLength(8)])
      });
  }

  onSubmit(): void {
      this.submitted = true;
  }
}

/** Error when invalid control is dirty, touched, or submitted. */
export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}
