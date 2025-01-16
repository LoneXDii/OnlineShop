import {CartProduct} from './cartProduct.interface';

export interface Order {
  id: string;
  orderStatus: string;
  paymentStatus: string;
  userId: string;
  totalPrice: number;
  createdAt: string;
  products: CartProduct[];
}
