import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import {  GetOrderDto} from '../models/order.model';
import { environment } from '../../environment/environment';


@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.serverUrl}/api/order`;

  GetAllOrders(): Observable<GetOrderDto[]> {
    return this.http.get<GetOrderDto[]>(this.apiUrl);
  }
  getOrderHistory(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/history/${userId}`);
  }
}
