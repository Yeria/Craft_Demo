import { Injectable } from "@angular/core";
import { RestService } from './rest.service';
import { AppConfig } from '../app.config';
import { User } from '../models';

@Injectable()
export class UserService {
    constructor(private readonly restService: RestService,
        private readonly appConfig: AppConfig) {}

    getUser(id: number, version: number = 1): Promise<User> {
        //return this.restService.get(`user/getuser?id=${id}`, `${this.appConfig.apiUrl}/v${version}`);
        return this.restService.get(`/getuser?id=${id}`, `${this.appConfig.springApiUrl}`);
    }

    testSpring(): Promise<any> {
        return this.restService.get("", "https://localhost:8443/loginTest");
    }
}