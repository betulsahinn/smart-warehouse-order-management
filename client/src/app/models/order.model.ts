export enum OrderStatus {
  Draft = 0,
  Confirmed = 1,
  Picking = 2,
  Packed = 3,
  Shipped = 4,
  Delivered = 5,
  Cancelled = 6,
}

export interface OrderItem {
  id: string;
  productId: string;
  productName: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface Order {
  id: string;
  orderNumber: string;
  customerId: string;
  warehouseId: string;
  status: OrderStatus;
  totalAmount: number;
  items: OrderItem[];
}
