import { Injectable } from "@angular/core";
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class AppConfig {
    
    constructor(private _snackBar: MatSnackBar) {}
    public readonly apiUrl: string = "https://localhost:44304/api";
    public readonly springApiUrl: string = "http://localhost:8080";

    openSnackBar(message: string, action: string) {
        this._snackBar.open(message, action, {
          duration: 20000,
        });
    }
}