import { Injectable } from "@angular/core";
import { AppConfig } from "../app.config";
//import { StorageService } from "./storage.service";

@Injectable()
export class SpinnerService {

    private counter: number;

    constructor(private readonly config: AppConfig) {
        this.counter = 1;

        if (typeof window !== 'undefined')
            this.counter = 0;
    }

    show() {
        this.counter++;
    }

    hide() {
        this.counter--;
        if (this.counter < 0) {
            this.counter = 0;
        }
    }

    get isLoading(): boolean {
        return this.counter > 0;
    }
} 