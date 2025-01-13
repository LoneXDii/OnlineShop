import {Routes} from '@angular/router';
import {OrdersListComponent} from './orders-list/orders-list.component';
import {canActivateAuth} from '../../data/guards/auth.guard';
import {OrderInfoComponent} from './order-info/order-info.component';

export const ordersRoutes: Routes = [
  {path: 'profile/orders', component: OrdersListComponent, canActivate:[canActivateAuth]},
  {path: 'orders/:id', component: OrderInfoComponent, canActivate:[canActivateAuth]},
]
