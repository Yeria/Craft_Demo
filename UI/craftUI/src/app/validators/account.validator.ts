import { AbstractControl, ValidationErrors, Validators } from "@angular/forms";

export class AccountValidator {
    static password(control: AbstractControl): ValidationErrors | null {

        let val = control.value as string;
        let isValid = val && val.length >= 6 && /\d/.test(val);
        return isValid ? null : { "password": "Please enter a valid password" };
    }

    static emailOptional(control: AbstractControl): ValidationErrors | null {
        if (!control.value)
            return null;

        return Validators.email(control);
    }
}