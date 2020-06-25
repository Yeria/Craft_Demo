import { Injectable } from "@angular/core";
import { RestService } from './rest.service';
import { NetworthRequest } from '../models';
import { NetworthResult } from '../models/networthResult.model';
import { AppConfig } from '../app.config';
import { FinancialEntity } from '../models/financialEntity.model';

@Injectable()
export class CalculationService {

    constructor(private readonly restService: RestService,
        private readonly appConfig: AppConfig) {

    }

    calculateNetworth(assets: FinancialEntity[], liabilities: FinancialEntity[], version: number = 1): Promise<NetworthResult> {
        var temp = new NetworthRequest(assets, liabilities);
        // return this.restService.post(temp, `${this.appConfig.apiUrl}/v${version}/calculator/calculatenetworth`).toPromise();
        return this.restService.post(temp, `${this.appConfig.springApiUrl}/calculatenetworth`);
    }
}