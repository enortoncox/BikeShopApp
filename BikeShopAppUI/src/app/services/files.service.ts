import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.development";

@Injectable({
    providedIn: "root"
})
export class FilesService{

    apiURL: string = environment.domain + "api/file"

    constructor(private http: HttpClient) {}

    UploadFile(formData: FormData): Observable<any>
    {
        return this.http.post(`${this.apiURL}`, formData)
    }

    UpdateFile(formData: FormData, oldFilePath: string): Observable<any>
    {
        return this.http.put(`${this.apiURL}`, formData, {params: {oldFilePath}})
    }

    DeleteFile(filePath: string): Observable<any>
    {
        return this.http.delete(`${this.apiURL}`, {params: {filePath}})
    }
}