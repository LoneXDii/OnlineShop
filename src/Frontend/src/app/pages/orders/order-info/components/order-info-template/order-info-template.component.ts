import {Component, Input} from '@angular/core';
import {DatePipe} from "@angular/common";
import {OrderProductComponent} from "../order-product/order-product.component";
import {Order} from '../../../../../data/interfaces/cart/order.interface';

@Component({
  selector: 'app-order-info-template',
    imports: [
        DatePipe,
        OrderProductComponent
    ],
  templateUrl: './order-info-template.component.html',
  styleUrl: './order-info-template.component.css'
})
export class OrderInfoTemplateComponent {
  @Input() order!: Order;
}
