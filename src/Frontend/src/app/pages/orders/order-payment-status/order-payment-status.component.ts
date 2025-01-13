import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute, RouterLink} from '@angular/router';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-order-payment-status',
  imports: [
    NgIf,
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
