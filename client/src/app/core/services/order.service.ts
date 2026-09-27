import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Order } from '../../models/order.model';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private readonly endpoint = `${environment.apiBaseUrl}/api/orders`;

  constructor(private readonly http: HttpClient) {}

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.endpoint);
  }
}
