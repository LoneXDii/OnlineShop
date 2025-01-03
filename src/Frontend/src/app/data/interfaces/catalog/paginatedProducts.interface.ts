import {Product} from './product.interface';

export interface PaginatedProducts {
  items: Product[];
  currentPage: number;
  totalPages: number;
}
