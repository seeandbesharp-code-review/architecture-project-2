import { Component, effect, EventEmitter, inject, input, Output, signal } from '@angular/core';
import { DonorService } from '../../../service/donor.service';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { Donor } from '../../../models/donor.model';

const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

@Component({
  selector: 'app-update-donor',
  standalone: true,
  imports: [ButtonModule, DialogModule, InputTextModule, FormsModule, ToastModule],
  providers: [MessageService],
  templateUrl: './update-donor.html',
  styleUrl: './update-donor.scss',
})
export class UpdateDonor {
  private donorService = inject(DonorService);
  private messageService = inject(MessageService);

  visible = signal(false);
  submitted = false;
  @Output() donorSaved = new EventEmitter<void>();

  donor = {
    id: 0,
    firstName: '',
    lastName: '',
    eMail: ''
  };

  donorTest = input<Donor | null>(null);

  constructor() {
    effect(() => {
      const data = this.donorTest();
      if (data) {
        this.donor = {
          id: data.id,
          firstName: data.firstName,
          lastName: data.lastName,
          eMail: data.eMail
        };
        this.submitted = false;
        this.visible.set(true);
      }
    });
  }

  saveUpdate() {
    this.submitted = true;
    const { firstName, lastName, eMail } = this.donor;

    if (!firstName.trim() || !lastName.trim()) return;
    if (!eMail.trim() || !EMAIL_REGEX.test(eMail.trim())) return;

    this.donorService.updateDonor({
      id: this.donor.id,
      firstName: firstName.trim(),
      lastName: lastName.trim(),
      eMail: eMail.trim()
    }).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'הצלחה', detail: 'התורם עודכן' });
        this.donorSaved.emit();
        this.visible.set(false);
        this.submitted = false;
      },
      error: (err) => {
        const detail = err.error?.message || err.error || 'עדכון התורם נכשל';
        this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: String(detail) });
      }
    });
  }
}
