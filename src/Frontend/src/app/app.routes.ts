import { Routes } from '@angular/router';
import {accountRoutes} from './pages/account/account.routes';
import {adminRoutes} from './pages/admin/admin.routes';
import {crudsRoutes} from './pages/cruds/cruds.routes';
import {catalogRoutes} from './pages/catalog/catalog.routes';
import {profileRoutes} from './pages/profile/profile.routes';
import {cartRoutes} from './pages/cart/cart.routes';
import {ordersRoutes} from './pages/orders/orders.routes';
import {supportChatRoutes} from './pages/support-chat/support-chat.routes';

export const routes: Routes = [
  ...accountRoutes,
  ...profileRoutes,
  ...catalogRoutes,
  ...adminRoutes,
  ...crudsRoutes,
  ...cartRoutes,
  ...ordersRoutes,
  ...supportChatRoutes,
];
