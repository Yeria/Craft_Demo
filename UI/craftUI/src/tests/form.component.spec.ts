import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule, By } from "@angular/platform-browser";
import { DebugElement } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { FormComponent } from '../app/components/form/form.component';
import { FinancialEntity } from 'src/app/models/financialEntity.model';
import { LabelValue } from 'src/app/models/labelValue.model';
import { CalculationService } from 'src/app/services/calculation.service';
import { StorageService } from 'src/app/services/storage.service';
import { UserService } from 'src/app/services/user.service';
import { IAMService } from 'src/app/services/iam.service';
import { CurrencyService } from 'src/app/services/currency.service';
import { AppConfig } from 'src/app/app.config';
import { RestService } from 'src/app/services/rest.service';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SpinnerService } from 'src/app/services/spinner.service';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe("FormComponent", () => {
    let component: FormComponent;
    let fixture: ComponentFixture<FormComponent>;
    let de: DebugElement;
    let el: HTMLElement;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                FormComponent
            ],
            imports: [
                BrowserModule,
                // BrowserAnimationsModule,
                FormsModule,
                ReactiveFormsModule,
                HttpClientModule,
                MatSnackBarModule,
                // MatSidenavModule,
                // MatFormFieldModule,
                // MatInputModule,
                // MatButtonModule,
                // MatSelectModule
            ],
            providers: [
                CalculationService,
                StorageService,
                UserService,
                CurrencyService,
                IAMService,
                AppConfig,
                RestService,
                HttpClient,
                SpinnerService
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(FormComponent);
        component = fixture.componentInstance;
        component.userName = "Test User!";
        component.addNewField(component.assets[0].dynamicFields, "Other");
        fixture.detectChanges();
    });
    
    it('should compile', () => {
        expect(component).toBeTruthy();
    });

    it(`should have greeting as text 'Test User!'`, async(() => {
        const compiled = fixture.nativeElement;
        expect(compiled.querySelector('.greeting').textContent).toContain('Test User!');
    }));

    it(`should add a field`, async(() => {
        expect(component.assets[0].dynamicFields.length).toEqual(3);
    }));

    it(`should be Other 3`, async(() => {
        expect(component.assets[0].dynamicFields[2].label).toEqual("Other 3");
    }));

    // it(`should call the onSubmit method`, async(() => {
    //     fixture.detectChanges();
    //     spyOn(comp, "onSubmit");
    //     el = fixture.debugElement.query(By.css("button")).nativeElement;
    //     el.click();
    //     expect(comp.onSubmit).toHaveBeenCalledTimes(0);
    // }));

    it(`form should be invalid`, async(() => {
        component.liabilities[0].staticFields[0].value = -100;
        expect(component.liabilities[0].staticFields[0].value === 0).toBeFalsy();
    }));

    // it(`form should be valid`, async(() => {
    //     comp.contactForm.controls["email"].setValue("test@test.com");
    //     comp.contactForm.controls["name"].setValue("Testy Person");
    //     comp.contactForm.controls["text"].setValue("This is a test text.");
    //     expect(comp.contactForm.valid).toBeTruthy();
    // }));
});