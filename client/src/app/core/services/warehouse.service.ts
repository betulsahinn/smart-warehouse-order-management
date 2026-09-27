import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Warehouse } from '../../models/warehouse.model';

@Injectable({ providedIn: 'root' })
export class WarehouseService {
  private readonly endpoint = `${environment.apiBaseUrl}/api/warehouses`;

  constructor(private readonly http: HttpClient) {}

  getWarehouses(): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(this.endpoint);
  }
}
