import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import * as signalR from "@microsoft/signalr";

import {Order} from "../../models/order";
import {OrderStatus} from "../../models/order-status";

@Component({
  selector: 'app-order-dashboard',
  templateUrl: './order-dashboard.component.html',
  styleUrls: ['./order-dashboard.component.css']
})
export class OrderDashboardComponent implements OnInit, OnDestroy{
  @Input()
  public orders: Order[] = []

  private signalRHubConnection: signalR.HubConnection;

  constructor() {
    this.signalRHubConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:8085/ordering-hub")
      .withAutomaticReconnect()
      .build();

    this.signalRHubConnection.on("OrderCreated", (createdOrder: Order) => {
      console.log(createdOrder);
      const isExists = this.orders.find(order => order.id === createdOrder.id);
      if (isExists){
        return;
      }
      this.orders.push(createdOrder);
    })

    this.signalRHubConnection.on("OrderStatusUpdated", (newOrderStatus: OrderStatus, orderId: string) =>{
      this.orders = this.orders.map(order => {
        if (order.id === orderId){
          return {...order, orderStatus: newOrderStatus};
        }
        return order;
      })
    })
  }

  async ngOnInit(): Promise<void> {
    await this.signalRHubConnection.start()
      .then(() => {
        this.signalRHubConnection.invoke("JoinGroup", "Web Dashboard")
          .catch((err) => {
            return console.error(err.toString());
          });
      });
  }

  async ngOnDestroy(): Promise<void> {
    await this.signalRHubConnection.stop()
      .then(() => {
        this.signalRHubConnection.invoke("LeaveGroup", "Web Dashboard")
          .catch((err) => {
            return console.error(err.toString());
          });
      });
  }
}
