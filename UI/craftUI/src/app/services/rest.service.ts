import { HttpClient, HttpResponse, HttpHeaders } from "@angular/common/http";
//import { Serializer } from 'v8';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { AppConfig } from "../app.config";
import { StorageService } from './storage.service';
import { SpinnerService } from './spinner.service';

@Injectable()
export class RestService {
    constructor(
        private httpClient: HttpClient,
        private config: AppConfig,
        private readonly storageService: StorageService,
        private readonly spinnerService: SpinnerService
    ) {}

    private getHeaders(): HttpHeaders{
        var h = new HttpHeaders();
        h.append("content-type", "application/json");
        h.append("Authorization", `Bearer ${this.storageService.getSessionItem("token")}`);

        return h;
    }

    public get(action: string, apiUrl: string = this.config.apiUrl): Promise<any> {
        let res = this.httpClient.get(`${apiUrl}/${action}`, {"headers": new HttpHeaders({"Content-Type": "application/json", "Authorization": `Bearer ${this.storageService.getSessionItem("token")}`})}).toPromise();
        this.handleSpinner(res);
        return res;
    }

    public post(entity: any, apiUrl: string = this.config.apiUrl): Promise<any> {
        let res = this.httpClient.post(`${apiUrl}`, JSON.stringify(entity), {"headers": new HttpHeaders({"Content-Type": "application/json", "Authorization": `Bearer ${this.storageService.getSessionItem("token")}`})}).toPromise();
        this.handleSpinner(res);
        return res;
    }

    handleSpinner(p: Promise<any>) {
        this.spinnerService.show();

        p.then(val => {
            this.spinnerService.hide();
        }).catch(rej => {
            this.spinnerService.hide();
        });
    }
}