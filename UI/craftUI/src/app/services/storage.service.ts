import { Injectable } from "@angular/core";
import { AppConfig } from "../app.config";

@Injectable()
export class StorageService {

    loggedIn: boolean;

    constructor(private readonly config: AppConfig) {
        this.loggedIn = false;
    }

    setSessionItem(key: string, data: string): void {
        if (typeof window !== "undefined" && typeof sessionStorage !== "undefined") {
            //sessionStorage.removeItem(key);
            sessionStorage.setItem(key, data);

            //return;
        }
    }

    getSessionItem(key: string): string | null {
        if (typeof window !== "undefined" && sessionStorage.getItem(key) !== null) {
            return sessionStorage.getItem(key);
        }

        return null;
    }

    setItem(key: string, data: string): void {
        if (typeof window !== "undefined" && typeof localStorage !== "undefined") {
            
            localStorage.removeItem(key);
            localStorage.setItem(key, data);
            
            return;
        }

        //Todo: use cookie to set data if localStorage is not found
    }

    getItem(key: string): string | null {
        if (typeof window !== "undefined" && localStorage.getItem(key) !== null) {
            return localStorage.getItem(key);
        }

        return null;
    }

    removeItem(key: string): void {
        if (typeof window !== 'undefined') return localStorage.removeItem(key);
    }

    removeSessionItem(key: string): void {
        if (typeof window !== 'undefined') return sessionStorage.removeItem(key);
    }

    clearSession() {
        if (typeof window !== "undefined") {
            return sessionStorage.clear();
        }
    }
}