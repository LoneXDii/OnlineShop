import { Component } from '@angular/core';
import {AdminProductsComponent} from './admin-products/admin-products.component';
import {AdminCategoriesComponent} from './admin-categories/admin-categories.component';
import {AdminUsersComponent} from './admin-users/admin-users.component';
import {AdminOrdersComponent} from './admin-orders/admin-orders.component';

@Component({
  selector: 'app-admin',
  imports: [
    AdminProductsComponent,
    AdminCategoriesComponent,
    AdminUsersComponent,
    AdminOrdersComponent
  ],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent {
  status: string = 'products';

  changeStatus(newStatus: string): void {
    this.status = newStatus;
  }
}
