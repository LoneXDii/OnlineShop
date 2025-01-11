import {CartProduct} from './cartProduct.interface';

export interface Cart {
  count: number;
  totalCost: number;
  products: CartProduct[];
}
