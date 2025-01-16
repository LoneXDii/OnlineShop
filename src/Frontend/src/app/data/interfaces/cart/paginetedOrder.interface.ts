import {Order} from './order.interface';

export interface PaginatedOrder {
  items: Order[];
  currentPage: number;
  totalPages: number;
}
