import { Injector } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";

import * as m from "../models";

export class FormbaseComponent {

    messages: string[];

    constructor(injector: Injector) { }

    readonly masks = {
        phoneNumber: {
            mask: ["(", /\d/, /\d/, /\d/, ")", " ", /\d/, /\d/, /\d/, "-", /\d/, /\d/, /\d/, /\d/],
            guide: false
        },
        postalCode: {
            mask: [/[A-Za-z]/, /\d/, /[A-Za-z]/, " ", /\d/, /[A-Za-z]/, /\d/],
            guide: false
        }
    };

    validateAllFormFields(formGroup: FormGroup) {
        Object.keys(formGroup.controls).forEach(field => {
            const control = formGroup.get(field);
            if (control instanceof FormControl) {
                control.markAsDirty({ onlySelf: true });
            } else if (control instanceof FormGroup) {
                this.validateAllFormFields(control);
            }
        });
    }

    showGeneralAlert(response: any = null) {
        if (response != null) {
            if (response.status === 400) {
                //let errorData = JSON.parse(response._body) as m.ValidationError;
                let errorData = JSON.parse(response._body);
                this.displayFromResult(errorData);
            } else {
                this.displayString("serverError");
            }
        }
    }

    private displayFromResult(data: any) {
        this.displayStrings(data.generalErrors.map(x => x.error));
    }

    private displayString(message: string) {
        this.displayStrings([message]);
    }

    private displayStrings(messages: string[]) {
        this.messages = messages;
    }
}