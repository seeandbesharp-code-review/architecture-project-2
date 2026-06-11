import { Component, OnInit, inject, signal, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { OrderService } from '../../../service/order.service.';
import { UserService } from '../../../service/user.service';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-history',
  standalone: true,
  imports: [CommonModule, ToastModule],
  providers: [MessageService],
  templateUrl: './order-history.html',
  styleUrls: ['./order-history.scss']
})
export class OrderHistory implements OnInit {
  private orderService = inject(OrderService);
  private userService = inject(UserService);
  private platformId = inject(PLATFORM_ID);
  private messageService = inject(MessageService);
  private router = inject(Router);

  history = signal<any[]>([]);
  openedOrderId: number | null = null;

  ngOnInit(): void {
    this.loadHistory();
  }

  toggleOrder(orderId: number) {
    this.openedOrderId = this.openedOrderId === orderId ? null : orderId;
  }

  loadHistory() {
    if (!isPlatformBrowser(this.platformId)) return;

    const userId = this.userService.currentUser()?.id;
    if (!userId) {
      this.router.navigate(['/login']);
      return;
    }

    this.orderService.getOrderHistory(userId).subscribe({
      next: (res) => this.history.set(res),
      error: () => this.messageService.add({
        severity: 'error', summary: 'שגיאה', detail: 'טעינת ההיסטוריה נכשלה'
      })
    });
  }
}
