import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { GetOrderDto } from '../../../models/order.model';
import { OrderService } from '../../../service/order.service.';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, TableModule, ToastModule],
  providers: [MessageService],
  templateUrl: './order-list.html',
  styleUrl: './order-list.scss'
})
export class OrderList implements OnInit {
  private orderService = inject(OrderService);
  private messageService = inject(MessageService);

  orders = signal<GetOrderDto[]>([]);

  ngOnInit(): void {
    this.loadAllOrders();
  }

  loadAllOrders(): void {
    this.orderService.GetAllOrders().subscribe({
      next: (data) => this.orders.set(data),
      error: () => this.messageService.add({
        severity: 'error', summary: 'שגיאה', detail: 'טעינת ההזמנות נכשלה'
      })
    });
  }

  totalRevenue = computed(() =>
    this.orders().reduce((sum, order) => sum + (order.gift?.price || 0), 0)
  );

  averageOrderValue = computed(() => {
    const count = this.orders().length;
    return count > 0 ? this.totalRevenue() / count : 0;
  });

  winnersCount = computed(() =>
    this.orders().filter(o => o.win).length
  );
}
