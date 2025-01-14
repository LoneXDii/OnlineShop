import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute, RouterLink} from '@angular/router';

@Component({
  selector: 'app-order-payment-status',
  imports: [
    RouterLink
  ],
  templateUrl: './order-payment-status.component.html',
  styleUrl: './order-payment-status.component.css'
})
export class OrderPaymentStatusComponent implements OnInit {
  route = inject(ActivatedRoute);
  paymentStatus: string | null = null;

  ngOnInit() {
    this.route.params
      .subscribe(params => this.paymentStatus = params['status']);
  }
}
