export interface Product {
  id: string;
  sku: string;
  name: string;
  description: string | null;
  unitPrice: number;
  reorderLevel: number;
  isActive: boolean;
}
