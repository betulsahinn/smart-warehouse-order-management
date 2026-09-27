import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product } from '../../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly endpoint = `${environment.apiBaseUrl}/api/products`;

  constructor(private readonly http: HttpClient) {}

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.endpoint);
  }
}
