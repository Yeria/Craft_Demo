import { Injectable } from "@angular/core";
import { RestService } from './rest.service';
import { NetworthRequest } from '../models';
import { NetworthResult } from '../models/networthResult.model';
import { AppConfig } from '../app.config';
import { CurrencyType } from '../models/currency.type';

@Injectable()
export class CurrencyService {
    private rateMatrix: number[][] = new Array<Array<number>>(3);

    constructor(private readonly restService: RestService,
        private readonly appConfig: AppConfig) {
        
        this.initRateMatrix();
    }

    private initRateMatrix(): void {
        this.rateMatrix[CurrencyType.CDN] = new Array<number>(3);
        this.rateMatrix[CurrencyType.CDN][CurrencyType.CDN] = 1;
        this.rateMatrix[CurrencyType.CDN][CurrencyType.USD] = 0.75;
        this.rateMatrix[CurrencyType.CDN][CurrencyType.EUR] = 0.65;

        this.rateMatrix[CurrencyType.USD] = new Array<number>(3);
        this.rateMatrix[CurrencyType.USD][CurrencyType.CDN] = 1.35;
        this.rateMatrix[CurrencyType.USD][CurrencyType.USD] = 1;
        this.rateMatrix[CurrencyType.USD][CurrencyType.EUR] = 0.85;

        this.rateMatrix[CurrencyType.EUR] = new Array<number>(3);
        this.rateMatrix[CurrencyType.EUR][CurrencyType.CDN] = 1.55;
        this.rateMatrix[CurrencyType.EUR][CurrencyType.USD] = 1.15;
        this.rateMatrix[CurrencyType.EUR][CurrencyType.EUR] = 1;
    }

    convertCurrency(value: number, fromCurr: CurrencyType, toCurr: CurrencyType): number {
        return value * this.rateMatrix[fromCurr][toCurr];
    }
}