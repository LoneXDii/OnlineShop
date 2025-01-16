import {ProductInfoComponent} from './product/product-info/product-info.component';
import {ProductCreationComponent} from './product/product-creation/product-creation.component';
import {canActivateAdmin} from '../../data/guards/admin.guard';
import {ProductEditingComponent} from './product/product-editing/product-editing.component';
import {CategoryInfoComponent} from './category/category-info/category-info.component';
import {CategoryCreationComponent} from './category/category-creation/category-creation.component';
import {CategoryEditingComponent} from './category/category-editing/category-editing.component';
import {Routes} from '@angular/router';
import {OrderInfoAdminComponent} from './order/order-info-admin/order-info-admin.component';
import {UserOrdersAdminComponent} from './order/user-orders-admin/user-orders-admin.component';

export const crudsRoutes: Routes = [
  {path: 'products/:id/info', component: ProductInfoComponent},
  {path: 'products/create', component: ProductCreationComponent, canActivate: [canActivateAdmin]},
  {path: 'products/:id/edit', component: ProductEditingComponent, canActivate: [canActivateAdmin]},
  {path: 'categories/:id/info', component: CategoryInfoComponent, canActivate: [canActivateAdmin]},
  {path: 'categories/create', component: CategoryCreationComponent, canActivate: [canActivateAdmin]},
  {path: 'categories/:id/edit', component: CategoryEditingComponent, canActivate: [canActivateAdmin]},
  {path: 'orders/:id/admin', component: OrderInfoAdminComponent, canActivate: [canActivateAdmin]},
  {path: 'users/:id/orders', component: UserOrdersAdminComponent, canActivate: [canActivateAdmin]},
]
