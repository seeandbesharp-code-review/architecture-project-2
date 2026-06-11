import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../environment/environment';
import { CreateBagDto, GetBagDto } from '../models/bag.model';


@Injectable({
  providedIn: 'root'
})
export class BagService {

   private http = inject(HttpClient);
  private apiUrl = `${environment.serverUrl}/api/Bag`;

  getAllBag(): Observable<GetBagDto[]> {
    return this.http.get<GetBagDto[]>(this.apiUrl);
  }

addBag(bag: CreateBagDto): Observable<CreateBagDto> {
  return this.http.post<CreateBagDto>(`${this.apiUrl}/add`, bag);
}
    deleteBag(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getBagsByUserId(userId: number): Observable<GetBagDto[]> {
  return this.http.get<GetBagDto[]>(`${this.apiUrl}/user/${userId}`);
}

  ProcessCheckout(userId: number): Observable<any> {
  return this.http.post(`${this.apiUrl}/checkout/${userId}`, {});
}
}