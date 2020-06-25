import { Component, Injector } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { StorageService } from 'src/app/services/storage.service';
import { Router } from '@angular/router';
import { AccountValidator } from 'src/app/validators/account.validator';
import { IAMService } from 'src/app/services/iam.service';
import { User } from 'src/app/models';
import { AppConfig } from 'src/app/app.config';

@Component({
    templateUrl: "./create-account.component.html",
    selector: "create-account",
    styleUrls: ['./create-account.component.scss']
})
export class CreateAccountComponent {
    formData: FormGroup;
    memTypes = [{"name": "Member", "value": "M"}, {"name": "Premium", "value": "P"}];

    constructor(private readonly storageService: StorageService,
        private readonly iamService: IAMService,
        private readonly formBuilder: FormBuilder,
        private readonly injector: Injector,
        private readonly appConfig: AppConfig,
        private readonly router: Router) {

        //super(injector);

        this.formData = this.formBuilder.group({
            "firstName": [null, [Validators.required]],
            "lastName": [null, [Validators.required]],
            "email": [null, [Validators.required, Validators.email]],
            "password": [null, [Validators.required, AccountValidator.password]],
            "memType": [null, [Validators.required]]
        });
    }

    submitData() {

        if (this.formData.valid) {
            var request = {
                id: 0,
                firstName: this.formData.value.firstName,
                lastName: this.formData.value.lastName,
                email: this.formData.value.email,
                password: this.formData.value.password,
                memberType: this.formData.value.memType,
                status: "A"
            } as User;

            this.iamService.createAccount(request, 1).then(r => {
                if (r){
                    this.storageService.removeSessionItem("userName");
                    this.appConfig.openSnackBar("Success!", "Close");
                    this.router.navigate(["/"]);
                } else 
                    alert("Didn't work! :(");
            }, err => {
                if (err.error.Message)
                    this.appConfig.openSnackBar(err.error.Message, "Close");
                else if(err.error.message)
                    this.appConfig.openSnackBar(JSON.parse(err.error.message.split("400 Bad Request:")[1])[0].Message, "Close");
                else
                    this.appConfig.openSnackBar("Error calculating networth. Please try again later.", "Close");
            });
        }
    }
}