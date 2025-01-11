import {canActivateAdmin} from '../../data/guards/admin.guard';
import {CartComponent} from './cart.component';
import {Routes} from '@angular/router';

export const cartRoutes: Routes = [
  {path: 'cart', component: CartComponent, canActivate: [canActivateAdmin]},
]
