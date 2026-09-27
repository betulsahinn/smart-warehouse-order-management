import { Routes } from '@angular/router';
import { DashboardPage } from './pages/dashboard/dashboard';
import { OrdersPage } from './pages/orders/orders';
import { ProductsPage } from './pages/products/products';
import { WarehousesPage } from './pages/warehouses/warehouses';

export const routes: Routes = [
  { path: '', component: DashboardPage, title: 'Dashboard | SWOMS' },
  { path: 'products', component: ProductsPage, title: 'Products | SWOMS' },
  { path: 'warehouses', component: WarehousesPage, title: 'Warehouses | SWOMS' },
  { path: 'orders', component: OrdersPage, title: 'Orders | SWOMS' },
  { path: '**', redirectTo: '' },
];
