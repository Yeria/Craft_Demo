import { Component, HostListener, Inject } from "@angular/core";
import { CalculationService } from 'src/app/services/calculation.service';
import { trigger, state, transition, style, animate } from '@angular/animations';
import { LabelValue } from 'src/app/models/labelValue.model';
import { StorageService } from 'src/app/services/storage.service';
import { UserService } from 'src/app/services/user.service';
import { CurrencyType } from 'src/app/models/currency.type';
import { CurrencyService } from 'src/app/services/currency.service';
import { FinancialEntity } from 'src/app/models/financialEntity.model';
import { IAMService } from 'src/app/services/iam.service';
import { AppConfig } from 'src/app/app.config';
import { DOCUMENT } from '@angular/common';

@Component({
    templateUrl: './form.component.html',
    selector: 'app-form',
    styleUrls: ['./form.component.scss'],
    animations:[ 
      trigger('fade',
      [ 
        state('void', style({ opacity : 0})),
        transition(':enter',[ animate(300)]),
        transition(':leave',[ animate(500)]),
      ]
  )]
})
export class FormComponent {
    private readonly prefixCDN = "CDN $";
    private readonly prefixUSD = "USD $";
    private readonly prefixEUR = "EUR €";

    prevCurrType = CurrencyType.CDN;
    currentCurrType = CurrencyType.CDN;
    currencyPrefix = this.prefixCDN;
    currencyTypes = [{"name": this.prefixCDN, "value": CurrencyType.CDN}, {"name": this.prefixUSD, "value": CurrencyType.USD}, {"name": this.prefixEUR, "value": CurrencyType.EUR}];
    assets = new Array<FinancialEntity>();
    liabilities = new Array<FinancialEntity>();
    
    totalAssets = 0;
    totalLiabilities = 0;
    netWorth = 0;
    userName = "";

    constructor(private readonly calculationService: CalculationService,
        private readonly storageService: StorageService,
        private readonly userService: UserService,
        private readonly currencyService: CurrencyService,
        private readonly iamService: IAMService,
        private readonly appConfig: AppConfig,
        @Inject(DOCUMENT) document) {
        this.initTestValues();

        if (this.storageService.getSessionItem("token")) {
            this.iamService.isAuthenticated().then(res => {
                if (res) {
                    let id = Number.parseInt(this.storageService.getSessionItem("id"));
                    this.userService.getUser(id).then(u => {
                        this.userName = u.firstName;
                    });
                }
            }, () => {
                this.storageService.removeSessionItem("token");
            });
        } else {
            this.userName = this.storageService.getSessionItem("userName");
        }
    }

    private initTestValues(): void {
        this.initAssets();
        this.initLiabilities();
    }

    private initAssets(): void {
        let cashAndInvestments = new FinancialEntity();
        cashAndInvestments.title = "Cash and Investments";
        
        cashAndInvestments.staticFields = new Array<LabelValue>();
        cashAndInvestments.dynamicFields = new Array<LabelValue>();
        
        cashAndInvestments.staticFields.push(new LabelValue("Chequing", 2000));
        cashAndInvestments.staticFields.push(new LabelValue("Saving for Taxes", 4000));
        cashAndInvestments.staticFields.push(new LabelValue("Rainy Day Fund", 506));
        cashAndInvestments.staticFields.push(new LabelValue("Savings for Fun", 5000));
        cashAndInvestments.staticFields.push(new LabelValue("Savings for Travel", 400));
        cashAndInvestments.staticFields.push(new LabelValue("Savings for Personal Development", 200));

        cashAndInvestments.dynamicFields.push(new LabelValue("Investment 1", 5000));
        cashAndInvestments.dynamicFields.push(new LabelValue("Investment 2", 60000));

        let longTermAssets = new FinancialEntity();
        longTermAssets.title = "Long Term Assets";
        
        longTermAssets.staticFields = new Array<LabelValue>();
        longTermAssets.dynamicFields = new Array<LabelValue>();

        longTermAssets.staticFields.push(new LabelValue("Primary Home", 455000));
        longTermAssets.staticFields.push(new LabelValue("Second Home", 1564321));

        longTermAssets.dynamicFields.push(new LabelValue("Other 1", 10000));
        longTermAssets.dynamicFields.push(new LabelValue("Other 2", 50000));

        this.assets.push(cashAndInvestments);
        this.assets.push(longTermAssets);
    }

    private initLiabilities(): void {
        let shortTermLiabilities = new FinancialEntity();
        shortTermLiabilities.title = "Short Term Liability";

        shortTermLiabilities.staticFields = new Array<LabelValue>();
        shortTermLiabilities.dynamicFields = new Array<LabelValue>();
        
        shortTermLiabilities.staticFields.push(new LabelValue("Credit Card 1", 4342));
        shortTermLiabilities.staticFields.push(new LabelValue("Credit Card 2", 4342));

        shortTermLiabilities.dynamicFields.push(new LabelValue("Other 1", 4342));

        let longTermDebt = new FinancialEntity();
        longTermDebt.title = "Long Term Debt";

        longTermDebt.staticFields = new Array<LabelValue>();
        longTermDebt.dynamicFields = new Array<LabelValue>();
        
        longTermDebt.staticFields.push(new LabelValue("Line of Credit", 10000));
        longTermDebt.staticFields.push(new LabelValue("Investment Loan", 10000));

        longTermDebt.dynamicFields.push(new LabelValue("Mortgage 1", 250999));
        longTermDebt.dynamicFields.push(new LabelValue("Mortgage 2", 632634));

        this.liabilities.push(shortTermLiabilities);
        this.liabilities.push(longTermDebt);
    }

    private convert(amount: number, fromCurr: CurrencyType, toCurr: CurrencyType): number {
        return Math.round(this.currencyService.convertCurrency(amount, fromCurr, toCurr));
    }

    onCurrencyChange(selection: any) {
        let v = selection.value;
        switch(v) {
            case CurrencyType.CDN: {
                this.currencyPrefix = this.prefixCDN;
                break;
            }
            case CurrencyType.USD: {
                this.currencyPrefix = this.prefixUSD;
                break;
            }
            case CurrencyType.EUR: {
                this.currencyPrefix = this.prefixEUR;
                break;
            }
            default: {
                break;
            }
        }

        this.prevCurrType = this.currentCurrType;
        this.currentCurrType = v;

        this.assets.forEach(asset => {
            asset.staticFields = asset.staticFields.map(sf => new LabelValue(sf.label, this.convert(sf.value, this.prevCurrType, this.currentCurrType)));
            asset.dynamicFields = asset.dynamicFields.map(df => new LabelValue(df.label, this.convert(df.value, this.prevCurrType, this.currentCurrType)));
        });
        
        this.liabilities.forEach(liability => {
            liability.staticFields = liability.staticFields.map(sf => new LabelValue(sf.label, this.convert(sf.value, this.prevCurrType, this.currentCurrType)));
            liability.dynamicFields = liability.dynamicFields.map(df => new LabelValue(df.label, this.convert(df.value, this.prevCurrType, this.currentCurrType)));
        });

        this.totalAssets = 0;
        this.totalLiabilities = 0;
        this.netWorth = 0;
    }

    validateInput(event: any) {
        if (event.srcElement.value < 0)
            event.srcElement.value = 0;
    }

    addNewField(targetList: LabelValue[], labelPrefix: string): void {
        targetList.push(new LabelValue(`${labelPrefix} ${targetList.length + 1}`, 0));
    }

    removeLastField(targetList: LabelValue[]): void {
        targetList.pop();
    }

    submitData(): void {
        
        this.calculationService.calculateNetworth(this.assets, this.liabilities).then(r => {
            this.totalAssets = r.totalAssets;
            this.totalLiabilities = r.totalLiabilities;
            this.netWorth = r.netWorth;
        
        }, err => {
            if (err.error.Message)
                this.appConfig.openSnackBar(err.error.Message, "Close");
            else if(err.error.message)
                this.appConfig.openSnackBar(JSON.parse(err.error.message.split("400 Bad Request:")[1])[0].Message, "Close");
            else
                this.appConfig.openSnackBar("Error calculating networth. Please try again later.", "Close");
        });
    }

    @HostListener('window:scroll', ['$event'])
    onWindowScroll(e) {
        if (window.pageYOffset > 350) {
        let element = document.getElementById('navbar');
        element.classList.add('sticky');
        } else {
        let element = document.getElementById('navbar');
            element.classList.remove('sticky'); 
        }
    }
}
