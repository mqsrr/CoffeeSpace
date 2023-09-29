import {Component, Input} from '@angular/core';
import {Order} from "../../models/order";
import {OrderStatus} from "../../models/order-status";

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent {
  @Input()
  public order: Order;

  constructor() {
    this.order = {
      id: '',
      orderItems: [],
      orderStatus: OrderStatus.Canceled,
      address: {
        id: '',
        city: '',
        country: '',
        street: ''
      }
    }
  };

  public getOrderStatusString(status: OrderStatus): string {
    switch (status) {
      case OrderStatus.Submitted:
        return 'Submitted';
      case OrderStatus.StockConfirmed:
        return 'Stock Confirmed';
      case OrderStatus.Paid:
        return 'Paid';
      case OrderStatus.Shipped:
        return 'Shipped';
      case OrderStatus.Canceled:
        return 'Canceled';
      default:
        return 'Unknown';
    }
  }
}
