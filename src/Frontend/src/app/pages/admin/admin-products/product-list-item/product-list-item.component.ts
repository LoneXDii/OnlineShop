import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {Product} from '../../../../data/interfaces/catalog/product.interface';
import {ProductsService} from '../../../../data/services/products.service';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-product-list-item',
  imports: [
    RouterLink
  ],
  templateUrl: './product-list-item.component.html',
  styleUrl: './product-list-item.component.css'
})
export class ProductListItemComponent {
  @Input() product!: Product;
  @Output() productDeleted = new EventEmitter<void>();
  productsService = inject(ProductsService);

  onDelete(){
    this.productsService.deleteProduct(this.product.id)
      .subscribe(() => {
        alert('Product deleted successfully.');
        this.productDeleted.emit();
      });
  }
}
