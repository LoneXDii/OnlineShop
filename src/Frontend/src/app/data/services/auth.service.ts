import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {tap} from 'rxjs';
import {Tokens} from '../interfaces/auth/tokens.interface';
import {CookieService} from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  http = inject(HttpClient);
  cookieService = inject(CookieService);
  baseUrl = 'http://localhost:5000/accounts';
  accessToken: string | null = null;
  refreshToken: string | null = null;

  constructor() { }

  get isAuthenticated(){
    if (!this.accessToken){
      this.accessToken = this.cookieService.get('accessToken');
    }
    return !!this.accessToken;
  }

  login(payload: {email: string, password: string}) {
    return this.http.post<Tokens>(`${this.baseUrl}/login`, payload)
      .pipe(
        tap(val => {
          this.accessToken = val.accessToken;
          this.refreshToken = val.refreshToken;

          this.cookieService.set('accessToken', this.accessToken);
          this.cookieService.set('refreshToken', this.refreshToken);
        })
      );
  }
}
