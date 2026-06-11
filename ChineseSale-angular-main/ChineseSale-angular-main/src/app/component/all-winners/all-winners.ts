import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Winner } from '../../models/user.model';
import { RandomService } from '../../service/random.service';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-all-winners',
  standalone: true,
  imports: [CommonModule, TableModule, ToastModule],
  providers: [MessageService],
  templateUrl: './all-winners.html',
  styleUrl: './all-winners.scss',
})
export class AllWinners implements OnInit {
  private randomService = inject(RandomService);
  private messageService = inject(MessageService);
  allWinners = signal<Winner[]>([]);

  ngOnInit() {
    this.loadWinner();
  }

  loadWinner() {
    this.randomService.getWinners().subscribe({
      next: (data) => this.allWinners.set(data),
      error: () => this.messageService.add({
        severity: 'error', summary: 'שגיאה', detail: 'טעינת רשימת הזוכים נכשלה'
      })
    });
  }
}
