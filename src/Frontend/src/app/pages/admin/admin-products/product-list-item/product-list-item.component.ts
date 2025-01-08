import {Component, Input} from '@angular/core';
import {Product} from '../../../../data/interfaces/catalog/product.interface';

@Component({
  selector: 'app-product-list-item',
  imports: [],
  templateUrl: './product-list-item.component.html',
  styleUrl: './product-list-item.component.css'
})
export class ProductListItemComponent {
  @Input() product!: Product;
}
