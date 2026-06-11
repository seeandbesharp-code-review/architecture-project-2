import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../environment/environment';
import { AddGiftDto, Gift } from '../models/gift.model';


@Injectable({
  providedIn: 'root'
})
export class GiftService {

   private http = inject(HttpClient);
  private apiUrl = `${environment.serverUrl}/api/Gift`;

  getAllGifts(): Observable<Gift[]> {
    return this.http.get<Gift[]>(this.apiUrl);
  }

    addGift(formData: FormData): Observable<Gift> {
    return this.http.post<Gift>(this.apiUrl, formData);
    }

    deleteGift(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  
  updateGift(formData: FormData): Observable<Gift> {
    return this.http.put<Gift>(`${this.apiUrl}`, formData);
  }



}