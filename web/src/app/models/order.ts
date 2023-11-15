import {OrderStatus} from "./order-status";
import {OrderItem} from "./orderitem";
import {Address} from "./address";

export interface Order {
  id: string;
  orderStatus: OrderStatus;
  address: Address;
  orderItems: OrderItem[];
}
