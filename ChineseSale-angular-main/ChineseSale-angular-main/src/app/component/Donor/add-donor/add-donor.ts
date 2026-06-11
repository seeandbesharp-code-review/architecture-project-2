import { Component, EventEmitter, inject, Output, signal } from '@angular/core';
import { DonorService } from '../../../service/donor.service';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

@Component({
  selector: 'app-add-donor',
  standalone: true,
  imports: [FormsModule, InputTextModule, ButtonModule, DialogModule, ToastModule],
  providers: [MessageService],
  templateUrl: './add-donor.html',
  styleUrl: './add-donor.scss',
})
export class AddDonor {
  private donorService = inject(DonorService);
  private messageService = inject(MessageService);

  visible = signal(false);
  submitted = false;
  @Output() donorSaved = new EventEmitter<void>();

  newDonor = {
    firstName: '',
    lastName: '',
    eMail: ''
  };

  showDialog() {
    this.submitted = false;
    this.newDonor = { firstName: '', lastName: '', eMail: '' };
    this.visible.set(true);
  }

  saveDonor() {
    this.submitted = true;
    const { firstName, lastName, eMail } = this.newDonor;

    if (!firstName.trim()) return;
    if (!lastName.trim()) return;
    if (!eMail.trim() || !EMAIL_REGEX.test(eMail.trim())) return;

    this.donorService.addDonor({
      firstName: firstName.trim(),
      lastName: lastName.trim(),
      eMail: eMail.trim()
    }).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success', summary: 'הצלחה', detail: 'התורם נוסף בהצלחה'
        });
        this.donorSaved.emit();
        this.visible.set(false);
        this.submitted = false;
        this.newDonor = { firstName: '', lastName: '', eMail: '' };
      },
      error: (err) => {
        const detail = err.error?.message || err.error || 'הוספת התורם נכשלה';
        this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: String(detail) });
      }
    });
  }
}
