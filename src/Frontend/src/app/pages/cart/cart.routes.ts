import {CartComponent} from './cart.component';
import {Routes} from '@angular/router';
import {canActivateAuth} from '../../data/guards/auth.guard';

export const cartRoutes: Routes = [
  {path: 'cart', component: CartComponent, canActivate: [canActivateAuth]},
]
