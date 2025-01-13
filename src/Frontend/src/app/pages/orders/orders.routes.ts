import {Routes} from '@angular/router';
import {OrdersListComponent} from './orders-list/orders-list.component';
import {canActivateAuth} from '../../data/guards/auth.guard';

export const ordersRoutes: Routes = [
  {path: 'profile/orders', component: OrdersListComponent, canActivate:[canActivateAuth]},
]
