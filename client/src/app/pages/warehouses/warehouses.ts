import { Component, OnInit } from '@angular/core';
import { WarehouseService } from '../../core/services/warehouse.service';
import { Warehouse } from '../../models/warehouse.model';

@Component({
  selector: 'app-warehouses-page',
  templateUrl: './warehouses.html',
})
export class WarehousesPage implements OnInit {
  warehouses: Warehouse[] = [];
  loading = true;
  error = '';

  constructor(private readonly warehouseService: WarehouseService) {}

  ngOnInit(): void {
    this.warehouseService.getWarehouses().subscribe({
      next: (warehouses) => {
        this.warehouses = warehouses;
        this.loading = false;
      },
      error: () => {
        this.error =
          'Could not load warehouses. Check that the API is running and access is authorized.';
        this.loading = false;
      },
    });
  }
}
