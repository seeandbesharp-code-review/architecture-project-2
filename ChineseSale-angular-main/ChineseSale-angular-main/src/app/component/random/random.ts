import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { RandomService } from '../../service/random.service';
import { GiftService } from '../../service/gift.service';
import { Gift } from '../../models/gift.model';

@Component({
  selector: 'app-random',
  standalone: true,
  imports: [CommonModule, ButtonModule, ToastModule],
  providers: [MessageService],
  templateUrl: './random.html',
  styleUrl: './random.scss',
})
export class Random implements OnInit {
  private randomService = inject(RandomService);
  private giftService = inject(GiftService);
  private messageService = inject(MessageService);

  gifts = signal<Gift[]>([]);
  drawingId = signal<number | null>(null);

  ngOnInit() {
    this.giftService.getAllGifts().subscribe({
      next: (data) => this.gifts.set(data),
      error: () => this.messageService.add({
        severity: 'error', summary: 'שגיאה', detail: 'טעינת המתנות נכשלה'
      })
    });
  }

  onExecuteDraw(giftId: number) {
    this.drawingId.set(giftId);
    this.randomService.runDraw(giftId).subscribe({
      next: (winner) => {
        this.drawingId.set(null);
        const name = winner.user
          ? `${winner.user.firstName} ${winner.user.lastName}`
          : `#${winner.idUser}`;
        this.messageService.add({
          severity: 'success',
          summary: 'הגרלה הושלמה',
          detail: `הזוכה: ${name}`,
          life: 5000
        });
      },
      error: (err) => {
        this.drawingId.set(null);
        this.messageService.add({
          severity: 'error',
          summary: 'שגיאה בהגרלה',
          detail: err.error || 'אירעה שגיאה לא ידועה',
          life: 5000
        });
      }
    });
  }
}
