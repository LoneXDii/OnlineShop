import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, catchError, tap, throwError} from 'rxjs';
import {Tokens} from '../interfaces/auth/tokens.interface';
import {CookieService} from 'ngx-cookie-service';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  http = inject(HttpClient);
  cookieService = inject(CookieService);
  router = inject(Router);
  baseUrl = 'http://localhost:5000/';
  accessToken: string | null = null;
  refreshToken: string | null = null;

  private loggedIn = new BehaviorSubject<boolean>(this.isAuthenticated);
  loggedIn$ = this.loggedIn.asObservable();

  constructor() { }

  get isAuthenticated(){
    if (!this.accessToken){
      this.accessToken = this.cookieService.get('accessToken');
      this.refreshToken = this.cookieService.get('refreshToken');
    }
    return !!this.accessToken;
  }

  get getAccessToken(){
    if (!this.accessToken) {
      this.accessToken = this.cookieService.get('accessToken');
    }

    return this.accessToken;
  }

  login(payload: {email: string, password: string}) {
    return this.http.post<Tokens>(`${this.baseUrl}accounts/login`, payload)
      .pipe(
        tap(val => {
          this.saveTokens(val);
          this.loggedIn.next(true);
        })
      );
  }

  register(formData: FormData) {
    return this.http.post(`${this.baseUrl}accounts/register`, formData);
  }

  refreshAccessToken(){
    return this.http.get(`${this.baseUrl}tokens/refresh?refreshToken=${this.refreshToken}`,
      {responseType: "text"})
      .pipe(
        tap(val => {
          this.updateAccessToken(val);
        }),
        catchError(err => {
          this.logout();
          return throwError(err);
        })
      );
  }

  logout(){
    this.http.get(`${this.baseUrl}accounts/logout`);
    this.loggedIn.next(false);

    this.cookieService.delete('accessToken');
    this.cookieService.delete('refreshToken');
    this.accessToken = null;
    this.refreshToken = null;

    this.router.navigate(['/login']);
  }

  saveTokens(res: Tokens) {
    this.accessToken = res.accessToken;
    this.refreshToken = res.refreshToken;
    this.cookieService.set('accessToken', this.accessToken);
    this.cookieService.set('refreshToken', this.refreshToken);
  }

  updateAccessToken(token: string) {
    this.accessToken = token;
    this.cookieService.set('accessToken', this.accessToken);
  }

  confirmEmail(params: {email: string, code: string}) {
    return this.http.get(`${this.baseUrl}accounts/confirmation`, {
      params: {
        email: params.email,
        code: params.code
      },
      responseType: "text"
    });
  }
}
