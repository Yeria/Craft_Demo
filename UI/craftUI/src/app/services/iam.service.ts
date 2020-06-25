import { Injectable } from "@angular/core";
import { RestService } from './rest.service';
import { NetworthRequest, User } from '../models';
import { NetworthResult } from '../models/networthResult.model';
import { AppConfig } from '../app.config';
import { LoginResult } from '../models/loginResult.model';
import { LoginRequest } from '../models/loginRequest.model';
import { StorageService } from './storage.service';

@Injectable()
export class IAMService {

    constructor(private readonly restService: RestService,
        private readonly storageService: StorageService,
        private readonly appConfig: AppConfig) {

    }
    
    login(loginRequest: LoginRequest, version: number = 1): Promise<LoginResult> {
        //return this.restService.post(loginRequest, `${this.appConfig.apiUrl}/v${version}/iam/login`).toPromise();
        return this.restService.post(loginRequest, `${this.appConfig.springApiUrl}/login`);
    }

    createAccount(user: User, version: number = 1): Promise<boolean> {
        return this.restService.post(user, `${this.appConfig.apiUrl}/v${version}/iam/createuser`);
    }

    isAuthenticated(version: number = 1): Promise<boolean> {
        let token = this.storageService.getSessionItem("token");

        if (!token)
            return Promise.resolve(false);

        //let response = this.restService.get("", `${this.appConfig.apiUrl}/v${version}/iam/IsAuthenticated`);
        let response = this.restService.get("isauthenticated", `${this.appConfig.springApiUrl}`);

        return response.then(res => {
            if (res === "NA"){
                this.storageService.removeSessionItem("token");
                return Promise.resolve(false);
            } else {
                this.storageService.setSessionItem("token", res);
                return Promise.resolve(true);
            }
        }, err => {
            return Promise.resolve(false);
        });
    }
}