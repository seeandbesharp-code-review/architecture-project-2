import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { UserService } from '../../../service/user.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,
    CardModule,
    ToastModule,
    RouterLink
  ],
  providers: [MessageService],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  private messageService = inject(MessageService);
  private userService = inject(UserService);
  private router = inject(Router);

  isLoading = signal(false);

  loginData = {
    Email: '',
    Password: ''
  };

  onLogin() {
    if (!this.loginData.Email || !this.loginData.Password) {
      this.messageService.add({
        severity: 'warn',
        summary: 'שדות חסרים',
        detail: 'נא להזין אימייל וסיסמה'
      });
      return;
    }

    this.isLoading.set(true);

    this.userService.LogInUser(this.loginData).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.messageService.add({
          severity: 'success',
          summary: 'ברוכ/ה הבא/ה!',
          detail: 'התחברת בהצלחה'
        });
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.isLoading.set(false);
        const detail = err.status === 0
          ? 'לא ניתן להתחבר לשרver. ודא/י שה-API פועל.'
          : 'אימייל או סיסמה שגויים.';
        this.messageService.add({
          severity: 'error',
          summary: 'כניסה נכשלה',
          detail
        });
      }
    });
  }
}
