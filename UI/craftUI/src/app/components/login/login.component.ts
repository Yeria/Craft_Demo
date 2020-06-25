import { Component, Injector } from "@angular/core";
import { Router } from "@angular/router";
import { FormGroup, FormBuilder, FormControl, FormGroupDirective, NgForm, Validators } from "@angular/forms";
import { ErrorStateMatcher } from "@angular/material/core";
import { FormbaseComponent } from '../formbase.component';
import { AccountValidator } from "../../validators/account.validator";
import { StorageService } from 'src/app/services/storage.service';
import { LoginRequest } from 'src/app/models/loginRequest.model';
import { IAMService } from 'src/app/services/iam.service';
import { AppConfig } from 'src/app/app.config';
import { UserService } from 'src/app/services/user.service';

@Component({
    templateUrl: "./login.component.html",
    selector: "login",
    styleUrls: ['./login.component.scss'],
})
export class LoginComponent extends FormbaseComponent {
    formData: FormGroup;
    formDataAnonymous: FormGroup;
    userName = "";

    constructor(private readonly iamService: IAMService,
        private readonly storageService: StorageService,
        private readonly formBuilder: FormBuilder,
        private readonly injector: Injector,
        private readonly router: Router,
        private readonly appConfig: AppConfig) {

        super(injector);
        
        this.formData = this.formBuilder.group({
            "email": [null, [Validators.required, Validators.email]],
            "password": [null, [Validators.required, AccountValidator.password]]
        });

        this.formDataAnonymous = this.formBuilder.group({
            "anonymousName": [null, [Validators.required]]
        });
    }

    proceedAnonymously() {
        if (this.formDataAnonymous.valid){
            this.storageService.setSessionItem("userName", this.userName);
            this.router.navigate(["/form"]);
        }
    }

    submitData() {
        if (this.formData.valid) {
            var request = {
                userName: this.formData.value.email,
                password: this.formData.value.password
            } as LoginRequest;

            this.iamService.login(request).then(res => {
                if (res){
                    this.storageService.setSessionItem("id", res.user.id.toString());
                    this.storageService.setSessionItem("token", res.token);
                    this.router.navigate(["/form"]);
                } else {
                    this.appConfig.openSnackBar("Could not login", "Close");
                }
            }, err => {
                this.appConfig.openSnackBar("Could not login", "Close");
            });
            
        }
    }
}