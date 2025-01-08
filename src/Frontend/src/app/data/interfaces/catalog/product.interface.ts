import {Category} from './category.interface';
import {AttributeValue} from './attributeValue.interface';

export interface Product {
  id: number;
  name: string;
  price: number;
  quantity: number;
  imageUrl: string | null;
  discount: {
    id: number;
    percent: number;
  } | null;
  category: Category;
  attributeValues: AttributeValue[];
}
