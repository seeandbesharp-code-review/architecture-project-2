import { Component, inject, OnInit, signal, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DonorService } from '../../service/donor.service';
import { GiftService } from '../../service/gift.service';
import { UserService } from '../../service/user.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']
})
export class Home implements OnInit {
  private donorService = inject(DonorService);
  private giftService = inject(GiftService);
  private userService = inject(UserService);
  private platformId = inject(PLATFORM_ID);

  currentDate = new Date().toLocaleDateString('he-IL', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });

  stats = signal({ donors: 0, gifts: 0, totalValue: 0 });
  userName = signal('');

  ngOnInit() {
    const user = this.userService.currentUser();
    if (user) {
      this.userName.set(user.firstName);
    }

    if (!isPlatformBrowser(this.platformId)) return;

    this.donorService.getAllDonors().subscribe({
      next: (donors) => this.stats.update(s => ({ ...s, donors: donors.length }))
    });

    this.giftService.getAllGifts().subscribe({
      next: (gifts) => {
        const totalValue = gifts.reduce((sum, g) => sum + (g.price || 0), 0);
        this.stats.update(s => ({ ...s, gifts: gifts.length, totalValue }));
      }
    });
  }
}
