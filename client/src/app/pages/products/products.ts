import { DecimalPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-products-page',
  imports: [DecimalPipe],
  templateUrl: './products.html',
})
export class ProductsPage implements OnInit {
  products: Product[] = [];
  loading = true;
  error = '';

  constructor(private readonly productService: ProductService) {}

  ngOnInit(): void {
    this.productService.getProducts().subscribe({
      next: (products) => {
        this.products = products;
        this.loading = false;
      },
      error: () => {
        this.error =
          'Could not load products. Check that the API is running and access is authorized.';
        this.loading = false;
      },
    });
  }
}
