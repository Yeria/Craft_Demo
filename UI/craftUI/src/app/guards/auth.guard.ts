import { Injectable } from "@angular/core";
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";

import { IAMService } from '../services/iam.service';
import { FormComponent } from '../components/form/form.component';
import { StorageService } from '../services/storage.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private readonly iamService: IAMService,
        private readonly storageService: StorageService,
        private readonly router: Router) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

        return new Promise<boolean>((resolve) => {
            if (typeof window === "undefined") {
                resolve(false);
            } else {
                this.iamService.isAuthenticated()
                    .then(val => {
                        if (route.component === FormComponent) {
                            if (val) {
                                resolve(true);
                            } else if (!val && this.storageService.getSessionItem("userName")) {
                                this.storageService.removeSessionItem("token");
                                resolve(true);
                            } else {
                                resolve(false);
                                this.storageService.removeSessionItem("userName");
                                this.storageService.removeSessionItem("token");
                                this.router.navigate(["/"]);
                            }
                        } else if (val) {
                            resolve(true);
                        } else {
                            resolve(false);
                            this.storageService.removeSessionItem("userName");
                            this.storageService.removeSessionItem("token");
                        }
                    });
            }
        });
    }
}