import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, catchError, tap, throwError} from 'rxjs';
import {Tokens} from '../interfaces/auth/tokens.interface';
import {CookieService} from 'ngx-cookie-service';
import {Router} from '@angular/router';
import {DecodedToken} from '../interfaces/auth/decodedToken.interface';
import {jwtDecode} from 'jwt-decode';
import {CartService} from './cart.service';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  http = inject(HttpClient);
  cookieService = inject(CookieService);
  cartService = inject(CartService);
  router = inject(Router);
  baseUrl = `${environment.apiUrl}/`;
  accessToken: string | null = null;
  refreshToken: string | null = null;

  private loggedIn = new BehaviorSubject<boolean>(this.isAuthenticated);
  loggedIn$ = this.loggedIn.asObservable();

  private isAdminRole = new BehaviorSubject<boolean>(this.isAdmin);
  isAdmin$ = this.isAdminRole.asObservable();

  get isAuthenticated(){
    if (!this.accessToken){
      this.accessToken = this.cookieService.get('accessToken');
      this.refreshToken = this.cookieService.get('refreshToken');
    }

    return !!this.accessToken;
  }

  get isAdmin(){
    if (!this.accessToken) {
      if(!this.cookieService.get('accessToken')){
        return false;
      }
      this.accessToken = this.cookieService.get('accessToken');
    }

    const role = jwtDecode<DecodedToken>(this.accessToken)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

    return role === 'admin';
  }

  get getAccessToken(){
    if (!this.accessToken) {
      this.accessToken = this.cookieService.get('accessToken');
    }

    return this.accessToken;
  }

  get getUserId(){
    if (!this.accessToken) {
      this.accessToken = this.cookieService.get('accessToken');
    }

    return jwtDecode<DecodedToken>(this.accessToken).Id;
  }

  login(payload: {email: string, password: string}) {
    return this.http.post<Tokens>(`${this.baseUrl}accounts/login`, payload)
      .pipe(
        tap(val => {
          this.saveTokens(val);
          this.loggedIn.next(true);
          this.cartService.loadCartInfo();

          if (this.isAdmin){
            this.isAdminRole.next(true);
          }
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
    this.http.get(`${this.baseUrl}accounts/logout`)
      .subscribe(() => {
        this.cookieService.delete('refreshToken');
        this.cookieService.delete('accessToken');
        this.refreshToken = null;
        this.accessToken = null;

        this.cartService.loadCartInfo();

        this.loggedIn.next(false);
        this.isAdminRole.next(false);

        this.router.navigate(['/login']);
      })
      ;
  }

  saveTokens(res: Tokens) {
    this.cookieService.delete('refreshToken');
    this.cookieService.delete('accessToken');
    this.accessToken = res.accessToken;
    this.refreshToken = res.refreshToken;
    this.cookieService.set('accessToken', this.accessToken);
    this.cookieService.set('refreshToken', this.refreshToken);
  }

  updateAccessToken(token: string) {
    this.accessToken = token;
    this.cookieService.delete('accessToken');
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

  resendEmailConfirmationCode(email: string) {
    return this.http.get(`${this.baseUrl}accounts/confirmation/resend`, {params: {email: email}});
  }
}
