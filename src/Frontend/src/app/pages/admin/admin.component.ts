import {Component, inject, OnInit} from '@angular/core';
import {AdminProductsComponent} from './admin-products/admin-products.component';
import {AdminCategoriesComponent} from './admin-categories/admin-categories.component';
import {AdminUsersComponent} from './admin-users/admin-users.component';
import {AdminOrdersComponent} from './admin-orders/admin-orders.component';
import {ActivatedRoute, Router} from '@angular/router';

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
export class AdminComponent implements OnInit {
  status: string = 'products';
  router = inject(Router);
  route = inject(ActivatedRoute);

  ngOnInit() {
    this.route.params.subscribe(params => {
      const requestedTab = params['tab'];

      if(requestedTab) {
        this.status = requestedTab;
      }
    });
  }

  changeStatus(newStatus: string): void {
    this.router.navigate([`/admin/${newStatus}`]);
  }
}
