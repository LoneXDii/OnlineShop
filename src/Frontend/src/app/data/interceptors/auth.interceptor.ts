import {HttpHandlerFn, HttpInterceptorFn, HttpRequest} from '@angular/common/http';
import {inject} from '@angular/core';
import {AuthService} from '../services/auth.service';
import {BehaviorSubject, catchError, filter, switchMap, tap, throwError} from 'rxjs';

let isRefreshing = new BehaviorSubject<boolean>(false);

export const authTokenInterceptor : HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const accessToken = authService.accessToken;

  if(!accessToken){
    return next(req);
  }

  if (isRefreshing.value) {
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
  if (!isRefreshing.value) {
    isRefreshing.next(true);

    return authService.refreshAccessToken()
      .pipe(
        switchMap(res => {
          return next(addToken(req, res)).pipe(
            tap(() => isRefreshing.next(false))
          );
        })
      );
  }

  if (req.url.includes('refresh')){
    return next(addToken(req, authService.accessToken!));
  }

  return isRefreshing.pipe(
    filter(isRefreshing => !isRefreshing),
    switchMap(res => {
      return next(addToken(req, authService.accessToken!));
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
