import { Component, ElementRef, OnInit, ViewChild, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

// PrimeNG Imports
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { FileUploadModule } from 'primeng/fileupload';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputNumberModule } from 'primeng/inputnumber';
import { RadioButtonModule } from 'primeng/radiobutton';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { MessageService, ConfirmationService } from 'primeng/api';


// Services
import { GiftService } from '../../../service/gift.service';
import { CategoryService } from '../../../service/category.service'; 
import { DonorService } from '../../../service/donor.service';     
import { AddGiftDto, Gift, UpdateGiftDto } from '../../../models/gift.model';
import { Category } from '../../../models/category.model';
import { Donor } from '../../../models/donor.model';
import { DividerModule } from 'primeng/divider';
import { TextareaModule } from 'primeng/textarea';
import { environment } from '../../../../environment/environment';
import { GiftByCategory } from '../gift-by-category/gift-by-category';

import { CreateBagDto } from '../../../models/bag.model';
import { BagService } from '../../../service/bag.service';
import { RandomService } from '../../../service/random.service'; 
import { Winner } from '../../../models/user.model';
import { UserService } from '../../../service/user.service';


@Component({
  selector: 'app-list-gift',
  standalone: true,
  imports: [
    CommonModule, FormsModule, ButtonModule, ConfirmDialogModule, DialogModule, 
    FileUploadModule, IconFieldModule, InputIconModule, InputNumberModule, 
    RadioButtonModule, TableModule, ToastModule, ToolbarModule, InputTextModule,TextareaModule,
    SelectModule,DividerModule,GiftByCategory
  ],
  providers: [GiftService, CategoryService, DonorService, ConfirmationService],
  templateUrl: './list-gift.html',
  styleUrl: './list-gift.scss',
})
export class ListGift implements OnInit {
  private giftService = inject(GiftService);
  private categoryService = inject(CategoryService);
  private donerService = inject(DonorService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);
  readonly serverUrl =environment.serverUrl;
    private randomService = inject(RandomService);


products = signal<Gift[]>([]);       
  categories = signal<Category[]>([])  
  doners = signal<Donor[]>([]);       
  productDialog = signal<boolean>(false);
    private bagService = inject(BagService);

  
  product:Partial<Gift> = {};
  selectedProducts: any[] | null = null;
  submitted: boolean = false;
allWinners = signal<Winner[]>([]);


/////
  private userService = inject(UserService);
  user = this.userService.currentUser;
  isAdmin = computed(() => this.userService.currentUser()?.role === 'Admin');

  ngOnInit() {
    this.loadGifts();
    this.loadMetadata();
    this.loadWinner();
  }

  loadMetadata() {
    this.categoryService.getAllCategory().subscribe({
      next: (data) => {
        // console.log(data);
    this.categories.set(data);
      },
      error: () => console.error('חובה להוסיף קטגוריות !')
    });
    this.donerService.getAllDonors().subscribe({
      next: (data) => this.doners.set(data),
      error: () => console.error('חובה להוסיף תורמים !')
    });
  }

  loadGifts() {
    this.giftService.getAllGifts().subscribe({
      next: (data) => {
        // console.log(data);
        this.products.set(data);
      },
      error: () => this.showError('שגיאה בטעינת נתונים ')
    });
  }

    loadWinner() {
      this.randomService.getWinners().subscribe(data => {
    this.allWinners.set(data);
  });
  }

  openNew() {
    this.product = {
      name: '',
      description: '',
      price: 10,
      categoryId: undefined,
      idDoner: undefined,
      img: ''
    };
    this.resetFileInput();
    this.submitted = false;
    this.productDialog.set(true);
  }

  editProduct(product: any) {
    this.product = { ...product };
    this.productDialog.set(true);
  }




saveProduct() {
  this.submitted = true;

  const valid =
    !!this.product.name?.trim() &&
    !!this.product.categoryId &&
    !!this.product.idDoner &&
    (this.product.price ?? 0) > 0;

  if (!valid) {
    this.showError('נא למלא שם, קטגוריה, תורם ומחיר תקין');
    return;
  }
    
    const formData = new FormData();
    formData.append('name', this.product.name!);
    formData.append('price', String(this.product.price));
    formData.append('categoryId', String(this.product.categoryId));
    formData.append('idDoner', String(this.product.idDoner));

    if (this.selectedFile) {
      formData.append('image', this.selectedFile); 
    }

    if (this.product.description) {
      formData.append('description', this.product.description);
    }
    if (this.product.id) {
      formData.append('id', String(this.product.id));
      this.giftService.updateGift(formData).subscribe({
        next: () => { this.handleSuccess('המתנה עודכנה'); this.resetFileInput(); },
        error: (err) => this.handleError(err)
      });
    } else {
      this.giftService.addGift(formData).subscribe({
        next: () => { this.handleSuccess('מתנה נוספה בהצלחה'); this.resetFileInput(); },
        error: (err) => this.handleError(err)
      });
    }
  }



  

  deleteProduct(product: any) {
    this.confirmationService.confirm({
      message: `למחוק את ${product.name}?`,
      accept: () => {
        this.giftService.deleteGift(product.id).subscribe({
          next: () => { this.showSuccess('נמחק'); this.loadGifts(); },
          error: () => this.showError('מחיקה נכשלה')
        });
      }
    });
  }

  showSuccess(detail: string) {
    this.messageService.add({ severity: 'success', summary: 'הצלחה', detail, life: 3000 });
  }

  showError(detail: string) {
    this.messageService.add({ severity: 'error', summary: 'שגיאה', detail, life: 3000 });
  }

  hideDialog() { this.productDialog.set(false); 
    this.resetFileInput();
  }


  private handleSuccess(msg: string) {
    this.showSuccess(msg);
    this.loadGifts();
    this.productDialog.set(false);
    this.submitted = false;
  }

  private handleError(err: any) {
    console.error('Server Error:', err);
    
    let message = 'אירעה שגיאה בביצוע הפעולה';
    
    if (err.status === 500) {
      message = 'שגיאת שרת ';
    } else if (err.status === 400) {
      message = 'הנתונים שנשלחו אינם תקינים';
    }

    this.showError(message);
  }

// פונקציה לקבלת צבע הקטגוריה
getCategoryColor(categoryId: number | undefined): string {
  if (categoryId === undefined) return '#d4af37'; // צבע ברירת מחדל
  
  const category = this.categories().find(c => c.id === categoryId);
  return category?.color || '#d4af37'; 
}
selectedFile: File | null = null;

// פונקציה לטיפול בבחירת קובץ 
onFileSelected(event: any) {
  const file: File = event.target.files[0];
  if (file) {
    this.selectedFile = file;
  }
}


@ViewChild('fileInput') fileInput!: ElementRef;
//איפוס שדה הקובץ לאחר שמירה או ביטול
    resetFileInput() {
      this.selectedFile = null;
        if (this.fileInput && this.fileInput.nativeElement) {
            this.fileInput.nativeElement.value = '';
        }
    }

    //סינון מתנות לפי קטגוריה
  onFilterChanged(categoryId: number | null) {
  if (categoryId === null) {
    this.loadGifts(); 
  } else {
    this.filterByCategory(categoryId);
  }
}

filterByCategory(categoryId: number) {
  this.categoryService.getGiftsByCategory(categoryId).subscribe({
    next: (filteredGifts) => {
      this.products.set(filteredGifts);
    },
    error: (err) => {
      this.showError('לא הצלחנו לסנן את המתנות');
      console.error(err);
    }
  });
}

//הוספה לסל
  addGiftToCart(gift: any) {
    const userId = this.userService.currentUser()?.id;

    if (!userId) {
      this.showError('נא להתחבר למערכת');
      return;
    }

    const giftId = gift.id || gift.Id;

    if (giftId) {
      const bagToCreate: CreateBagDto = {
        idUser: Number(userId),
        idGift: Number(giftId),
        quantity: 1
      };

      this.bagService.addBag(bagToCreate).subscribe({
        next: () => this.showSuccess('המתנה נוספה לסל בהצלחה!'),
        error: (err) => {
          const detail = typeof err.error === 'string'
            ? err.error
            : err.error?.message || err.error?.title || 'שגיאה בהוספה לסל';
          this.showError(detail);
        }
      });
    } else {
      this.showError('מזהה מתנה חסר');
    }
  }



//ביצוע ההגרלה
onExecuteDraw(giftId: number) {
    this.randomService.runDraw(giftId).subscribe({
      next: (winner) => {
        this.loadWinner();
             this.allWinners.update(prev => [...prev, winner]);
        this.messageService.add({ 
          severity: 'success', 
          summary: 'בוצע בהצלחה', 
          detail: `נבחר זוכה למתנה! (: ${winner.idUser})`,
          life: 3000 
        });
// this.loadWinner();
      },
      error: (err) => {
        this.messageService.add({ 
          severity: 'error', 
          summary: 'שגיאה בביצוע ההגרלה', 
          detail: err.error || 'אירעה שגיאה לא ידועה', 
          life: 5000 
        });
      }
    });
  }

// מחזיר את הזוכה במתנה
  getWinner(giftId: number) {
return this.allWinners().find(winner => winner.idGift === giftId);}

}

