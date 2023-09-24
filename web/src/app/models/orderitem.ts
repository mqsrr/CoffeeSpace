import {OrderStatus} from "./order-status";

export interface OrderItem {
  id: string;
  buyerName: string;
  title: string;
  description: string;
  discount: number;
  quantity: number;
  price: number;
  orderStatus: OrderStatus;
  dateOfPurchase: string
}
