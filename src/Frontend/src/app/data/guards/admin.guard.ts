import {AuthService} from '../services/auth.service';
import {inject} from '@angular/core';
import {Router} from '@angular/router';

export const canActivateAdmin = () => {
  const isAdmin = inject(AuthService).isAdmin;

  if (isAdmin){
    return true;
  }

  return inject(Router).createUrlTree(['/profile']);
}
