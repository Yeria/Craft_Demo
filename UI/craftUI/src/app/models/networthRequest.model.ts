import { FinancialEntity } from './financialEntity.model';

export class NetworthRequest {  
    liabilities: FinancialEntity[];
    assets: FinancialEntity[];

    constructor(assets: FinancialEntity[], liabilities: FinancialEntity[]) {
        this.assets = assets;
        this.liabilities = liabilities;
    }
}