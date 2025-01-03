import {HttpHandlerFn, HttpInterceptorFn, HttpRequest} from '@angular/common/http';
import {inject} from '@angular/core';
import {AuthService} from '../services/auth.service';
import {catchError, switchMap, throwError} from 'rxjs';

let isRefreshing = false;

export const authTokenInterceptor : HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const accessToken = authService.accessToken;

  if(!accessToken){
    return next(req);
  }

  if (isRefreshing) {
    return refresh(authService, req, next);
  }

  return next(addToken(req, accessToken))
    .pipe(
      catchError(error => {
        if (error.status === 401){
          return refresh(authService, req, next);
        }

        return throwError(error);
      })
    );
}

const refresh = (
  authService: AuthService,
  req : HttpRequest<any>,
  next: HttpHandlerFn
) => {
  if (isRefreshing) {
    return next(addToken(req, authService.accessToken!));
  }

  isRefreshing = true;

  return authService.refreshAccessToken()
    .pipe(
      switchMap(res => {
        isRefreshing = false;
        return next(addToken(req, res));
      })
    );
}

const addToken = (req: HttpRequest<any>, accessToken: string) => {
  return req.clone({
    setHeaders: {
      Authorization: `Bearer ${accessToken}`
    }
  });
}
