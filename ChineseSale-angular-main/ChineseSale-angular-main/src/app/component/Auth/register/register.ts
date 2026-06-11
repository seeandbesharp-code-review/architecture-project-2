import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { UserService } from '../../../service/user.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule, FormsModule, ButtonModule, InputTextModule,
    PasswordModule, CardModule, ToastModule, RouterLink
  ],
  providers: [MessageService],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register {
  private messageService = inject(MessageService);
  private registerService = inject(UserService);
  private router = inject(Router);

  isLoading = signal(false);

  registerData = {
    FirstName: '',
    LastName: '',
    Email: '',
    Password: '',
    PhoneNumber: '',
    confirmPassword: ''
  };

  onRegister() {
    if (!this.registerData.FirstName || !this.registerData.LastName || !this.registerData.Email || !this.registerData.Password) {
      this.messageService.add({ severity: 'warn', summary: 'שדות חסרים', detail: 'נא למלא את כל השדות הנדרשים' });
      return;
    }

    if (this.registerData.Password !== this.registerData.confirmPassword) {
      this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'הסיסמאות אינן תואמות' });
      return;
    }

    this.isLoading.set(true);

    this.registerService.registerUser(this.registerData).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.messageService.add({
          severity: 'success',
          summary: 'הצלחה',
          detail: 'החשבון נוצר! מועבר/ת להתחברות'
        });
        setTimeout(() => this.router.navigate(['/login']), 1200);
      },
      error: (err) => {
        this.isLoading.set(false);
        let errorMessage = 'אירעה שגיאה ברישום';

        if (err.error?.errors) {
          const firstKey = Object.keys(err.error.errors)[0];
          errorMessage = err.error.errors[firstKey][0];
        } else if (err.error?.message) {
          errorMessage = err.error.message;
        } else if (typeof err.error === 'string') {
          errorMessage = err.error;
        }

        this.messageService.add({ severity: 'error', summary: 'שגיאה ברישום', detail: errorMessage });
      }
    });
  }
}
