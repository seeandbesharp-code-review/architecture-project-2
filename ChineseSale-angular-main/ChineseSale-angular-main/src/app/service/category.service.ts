import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';
import { AddCategoryDto, Category } from '../models/category.model';
import { Donor } from '../models/donor.model';
import { Gift } from '../models/gift.model';


@Injectable({
  providedIn: 'root'
})
export class CategoryService {

   private http = inject(HttpClient);
  private apiUrl = `${environment.serverUrl}/api/Category`;

  getAllCategory(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiUrl);
  }

    deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

    addCategory(category: AddCategoryDto): Observable<Category> {
      return this.http.post<Category>(this.apiUrl, category);
        }

     updateCategory(category: Category): Observable<Category> {
        return this.http.put<Category>(`${this.apiUrl}`, category);
      }

  getGiftsByCategory(categoryId: number): Observable<Gift[]> {
  return this.http.get<Gift[]>(`${this.apiUrl}/gift/${categoryId}`);
}
}