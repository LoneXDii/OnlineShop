import {Component, Input} from '@angular/core';
import {Product} from '../../../../data/interfaces/product.interface';
import {ProductCardComponent} from '../product-card/product-card.component';

@Component({
  selector: 'app-product-cards-layout',
  imports: [
    ProductCardComponent
  ],
  templateUrl: './product-cards-layout.component.html',
  styleUrl: './product-cards-layout.component.css'
})
export class ProductCardsLayoutComponent {
  @Input() products!: Product[];
}
