import {ProfileComponent} from './profile.component';
import {canActivateAuth} from '../../data/guards/auth.guard';
import {Routes} from '@angular/router';

export const profileRoutes: Routes = [
  {path: 'profile', component: ProfileComponent, canActivate: [canActivateAuth]}
]
