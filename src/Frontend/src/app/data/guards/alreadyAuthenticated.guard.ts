import {inject} from '@angular/core';
import {AuthService} from '../services/auth.service';
import {Router} from '@angular/router';

export const canActivateUnauthenticated = () => {
  const isLoggedIn = inject(AuthService).isAuthenticated;

  if (!isLoggedIn){
    return true;
  }

  return inject(Router).createUrlTree(['/profile']);
}
