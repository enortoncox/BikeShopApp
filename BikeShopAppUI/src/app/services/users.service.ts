import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { User } from "../interfaces/user";
import { UserPassword } from "../interfaces/userPassword";
import { environment } from "src/environments/environment.development";

@Injectable({
    providedIn: "root"
})
export class UsersService
{
    apiURL = environment.domain + "api/users";

    constructor(private http: HttpClient){}

    GetUser(userId: string): Observable<User>
    {
        return this.http.get<User>(`${this.apiURL}/${userId}`);
    }

    GetUsers(): Observable<User[]>
    {
        return this.http.get<User[]>(`${this.apiURL}`);
    }

    UpdateUser(updatedUser: User): Observable<any>
    {
        return this.http.put(`${this.apiURL}/${updatedUser.userId}`, updatedUser);
    }

    DeleteUser(userId: string): Observable<any>
    {
        return this.http.delete(`${this.apiURL}/${userId}`);
    }

    UpdateUserPassword(userPassword: UserPassword): Observable<any>
    {
        return this.http.put(`${this.apiURL}/${userPassword.userId}/password`, userPassword);
    }

    GetAllUsersThatStartWithLetter(letter: string): Observable<User[]>
    {
        return this.http.get<any>(`${this.apiURL}/name`, {params: {letter}});
    }
}