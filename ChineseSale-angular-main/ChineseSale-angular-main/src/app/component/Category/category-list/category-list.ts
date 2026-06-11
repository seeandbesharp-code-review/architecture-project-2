import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToolbarModule } from 'primeng/toolbar';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { CategoryService } from '../../../service/category.service';
import { Category, AddCategoryDto } from '../../../models/category.model';


@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, ButtonModule, DialogModule, ToolbarModule, InputTextModule, ToastModule],
  providers: [MessageService],
  templateUrl: './category-list.html',
  styleUrl: './category-list.scss'
})
export class CategoryList implements OnInit {
  private categoryService = inject(CategoryService);
  private messageService = inject(MessageService);
  isSaving: boolean = false;

  categories = signal<Category[]>([]);
  
  newCategory: AddCategoryDto = { name: '', color: '#3b82f6' };
  selectedCategory: Category = { id: 0, name: '', color: '' };

  addDialog: boolean = false;
  editDialog: boolean = false;
  submitted: boolean = false;

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getAllCategory().subscribe(data => this.categories.set(data));
  }

  // --- לוגיקת הוספה ---
  openAddDialog() {
    this.newCategory = { name: '', color: '#3b82f6' }; 
    this.submitted = false;
    this.addDialog = true;
  }

  saveNewCategory() {
    if (this.isSaving) return;
    this.submitted = true;
    if (!this.newCategory.name?.trim() || !this.newCategory.color) {
      this.showError('נא למלא שם וצבע');
      return;
    }
      this.isSaving = true;
      this.categoryService.addCategory(this.newCategory).subscribe({
        next: () => {
          this.showSuccess('הקטגוריה נוספה בהצלחה');
          this.addDialog = false;
          this.loadCategories();
          this.isSaving = false;
        },
        error: (err) => {
          this.showError('שגיאה בהוספה (ייתכן והשם קיים)');
          this.isSaving = false;  
        }
      });
  }

  // --- לוגיקת עדכון ---
  openEditDialog(category: Category) {
    this.selectedCategory = { ...category };
    this.submitted = false;
    this.editDialog = true;
  }

  saveUpdateCategory() {
    this.submitted = true;
    if (!this.selectedCategory.name?.trim()) {
      this.showError('נא למלא שם קטגוריה');
      return;
    }
    this.categoryService.updateCategory(this.selectedCategory).subscribe({
        next: () => {
          this.showSuccess('הקטגוריה עודכנה בהצלחה');
          this.editDialog = false;
          this.loadCategories();
        },
        error: (err) => this.showError('שגיאה בעדכון')
      });
  }

  // --- מחיקה ---
  deleteCategory(id: number) {
    this.categoryService.deleteCategory(id).subscribe({
     next: () => {
      this.showSuccess('הקטגוריה נמחקה ');
      this.loadCategories();
    },
    error: (err) => {
      console.error('Server Error:', err);
      
      if (err.status === 500) {
        this.showError('לא ניתן למחוק קטגוריה זו כיוון שהיא נמצאת בשימוש');
      } else {
        this.showError('אירעה שגיאה בלתי צפויה');
      }
    }
    });
  }

  private showSuccess(msg: string) {
    this.messageService.add({ severity: 'success', summary: 'הצלחה', detail: msg });
  }
  private showError(msg: string) {
    this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: msg });
  }
}