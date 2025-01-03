import {inject} from '@angular/core';
import {AuthService} from '../services/auth.service';
import {Router} from '@angular/router';

export const canActivateAuth = () =>{
  const isLoggedIn = inject(AuthService).isAuthenticated;

  if (isLoggedIn){
    console.log('logged in');
    return true;
  }

  console.log('not logged in');
  return inject(Router).createUrlTree(['/login']);
}
