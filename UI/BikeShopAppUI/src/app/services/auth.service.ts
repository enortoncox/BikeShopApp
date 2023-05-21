import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, ReplaySubject } from "rxjs";
import { UserLogin } from "../interfaces/userLogin";
import { User } from "../interfaces/user";
import { UserRegister } from "../interfaces/userRegister";
import { UserPassword } from "../interfaces/userPassword";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class AuthService{
    
    loggedInUser: ReplaySubject<User> = new ReplaySubject(1);
    isLoggedIn: boolean = false;
    isLoggedinAdmin: boolean = false;
    isCartEmpty: boolean = true;

    apiURL = "https://localhost:7000/api/auth";

    constructor(private http: HttpClient, private router: Router){}

    Login(userLogin: UserLogin): Observable<any>
    {
        return this.http.post<any>(`${this.apiURL}/login`, userLogin);
    }

    Register(userRegister: UserRegister): Observable<any>
    {
        return this.http.post<any>(`${this.apiURL}/register`, userRegister);
    }

    Logout()
    {
        this.isLoggedIn = false;
        this.loggedInUser.next({} as User);
        this.router.navigate(['/home']);
    }

    GetCurrentUser()
    {
        return this.loggedInUser.asObservable();
    }

    SetCurrentUser(newUser: User)
    {
        this.isLoggedIn = true;

        if(newUser.isAdmin)
        {
            this.isLoggedinAdmin = true;
        }
        else
        {
            this.isLoggedinAdmin = false;
        }

        this.loggedInUser.next(newUser);
    }

    ConfirmUserPassword(userPassword: UserPassword): Observable<boolean>
    {
        return this.http.post<boolean>(`${this.apiURL}/password`, userPassword);
    }

    IsUserAdmin(userId: string): Observable<boolean>
    {
        return this.http.get<boolean>(`${this.apiURL}/isadmin/${userId}`);
    }

    ChangeAdminStatus(userId: string): Observable<any>
    {
        return this.http.get<any>(`${this.apiURL}/isadmin/${userId}/change`);
    }
}