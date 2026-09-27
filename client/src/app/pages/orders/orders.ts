import { DecimalPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../core/services/order.service';
import { Order, OrderStatus } from '../../models/order.model';

@Component({
  selector: 'app-orders-page',
  imports: [DecimalPipe],
  templateUrl: './orders.html',
})
export class OrdersPage implements OnInit {
  orders: Order[] = [];
  loading = true;
  error = '';

  constructor(private readonly orderService: OrderService) {}

  ngOnInit(): void {
    this.orderService.getOrders().subscribe({
      next: (orders) => {
        this.orders = orders;
        this.loading = false;
      },
      error: () => {
        this.error =
          'Could not load orders. Check that the API is running and access is authorized.';
        this.loading = false;
      },
    });
  }

  getStatusLabel(status: OrderStatus): string {
    return OrderStatus[status] ?? 'Unknown';
  }
}
