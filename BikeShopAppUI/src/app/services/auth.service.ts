import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, ReplaySubject } from "rxjs";
import { UserLogin } from "../interfaces/userLogin";
import { User } from "../interfaces/user";
import { UserRegister } from "../interfaces/userRegister";
import { UserPassword } from "../interfaces/userPassword";
import { Router } from "@angular/router";
import { AuthenticationResponse } from "../interfaces/authenticationResponse";
import { VerifyTokenResponse } from "../interfaces/verifyTokenResponse";
import { environment } from "src/environments/environment.development";

@Injectable({
    providedIn: "root"
})
export class AuthService{
    
    loggedInUser: ReplaySubject<User> = new ReplaySubject(1);
    isLoggedIn: boolean;
    isLoggedInAdmin: boolean;
    isCartEmpty: boolean = true;
    apiURL = environment.domain + "api/auth";

    constructor(private http: HttpClient, private router: Router){
        this.isLoggedInAdmin = false;
        this.isLoggedIn = false;
    }

    Login(userLogin: UserLogin): Observable<AuthenticationResponse>
    {
        return this.http.post<AuthenticationResponse>(`${this.apiURL}/login`, userLogin);
    }

    Register(userRegister: UserRegister): Observable<AuthenticationResponse>
    {
        return this.http.post<AuthenticationResponse>(`${this.apiURL}/register`, userRegister);
    }

    Logout(redirectPath: string = "/home")
    {
        this.isLoggedIn = false;
        this.isLoggedInAdmin = false;
        this.loggedInUser.next({} as User);
        localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
        this.router.navigate([`${redirectPath}`]);
    }

    GetNewJwtToken(): Observable<AuthenticationResponse>
    {
        var token = localStorage["token"];
        var refreshToken = localStorage["refreshToken"];
        return this.http.post<AuthenticationResponse>(`${this.apiURL}/generate-new-jwt-token`, {jwtToken: token, refreshToken: refreshToken});
    }

    VerifyJwtToken(): Observable<VerifyTokenResponse>
    {
        var token = localStorage["token"];
        var refreshToken = localStorage["refreshToken"];
        return this.http.post<VerifyTokenResponse>(`${this.apiURL}/verify-jwt-token`, {jwtToken: token, refreshToken: refreshToken});
    }

    GetCurrentUser()
    {
        return this.loggedInUser.asObservable();
    }

    SetCurrentUser(newUser: User | null, isAdmin: boolean | null = false)
    {
        if(newUser == null)
        {
            this.loggedInUser.next({} as User);
            this.isLoggedIn = false;
            this.isLoggedInAdmin = false;
            return
        }

        this.isLoggedIn = true;

        if(isAdmin)
        {
            this.isLoggedInAdmin = true;
        }
        else
        {
            this.isLoggedInAdmin = false;
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