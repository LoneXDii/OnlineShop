import {Routes} from '@angular/router';
import {LoginComponent} from './login/login.component';
import {canActivateUnauthenticated} from '../../data/guards/alreadyAuthenticated.guard';
import {RegisterComponent} from './register/register.component';
import {ConfirmEmailComponent} from './confirm-email/confirm-email.component';
import {RefreshPasswordComponent} from './refresh-password/refresh-password.component';

export const accountRoutes: Routes = [
  {path: 'login', component: LoginComponent, canActivate: [canActivateUnauthenticated]},
  {path: 'register', component: RegisterComponent, canActivate: [canActivateUnauthenticated]},
  {path: 'confirm-email/:email', component: ConfirmEmailComponent},
  {path: 'refresh-password', component: RefreshPasswordComponent, canActivate: [canActivateUnauthenticated]}
]
