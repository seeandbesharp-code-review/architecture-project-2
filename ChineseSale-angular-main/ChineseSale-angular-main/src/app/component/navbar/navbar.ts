import { Component, inject, computed } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { CommonModule } from '@angular/common';
import { MenubarModule } from 'primeng/menubar';
import { ButtonModule } from 'primeng/button';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../service/user.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, MenubarModule, ButtonModule, RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  private userService = inject(UserService);
  private router = inject(Router);

  get currentUser() {
    return this.userService.currentUser;
  }

  items = computed<MenuItem[]>(() => {
    const currentUser = this.userService.currentUser();
    const isAdmin = currentUser?.role === 'Admin';
    const isLoggedIn = !!currentUser;

    return [
      { label: 'דף הבית', icon: 'pi pi-home', routerLink: '/' },
      { label: 'מתנות', icon: 'pi pi-gift', routerLink: '/gift' },
      { label: 'תורמים', icon: 'pi pi-users', routerLink: '/donor', visible: isAdmin },
      { label: 'קטגוריות', icon: 'pi pi-tags', routerLink: '/category', visible: isAdmin },
      { label: 'הזמנות', icon: 'pi pi-list', routerLink: '/order', visible: isAdmin },
      { label: 'הגרלות', icon: 'pi pi-sparkles', routerLink: '/random', visible: isAdmin },
      { label: 'זוכים', icon: 'pi pi-trophy', routerLink: '/winner' },
      { label: 'היסטוריה', icon: 'pi pi-history', routerLink: '/order-history', visible: isLoggedIn },
      {
        label: 'התנתק',
        icon: 'pi pi-sign-out',
        command: () => this.onLogout(),
        visible: isLoggedIn
      },
      { label: 'התחברות', icon: 'pi pi-sign-in', routerLink: '/login', visible: !isLoggedIn },
      { icon: 'pi pi-shopping-cart', routerLink: '/bag', visible: isLoggedIn }
    ];
  });

  onLogout() {
    this.userService.logout();
    this.router.navigate(['/login']);
  }
}
