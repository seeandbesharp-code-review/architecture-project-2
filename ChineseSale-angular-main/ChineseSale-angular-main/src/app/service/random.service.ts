import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';
import { Winner } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class RandomService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.serverUrl}/api/Random`;


  runDraw(giftId: number): Observable<Winner> {
    return this.http.post<Winner>(`${this.apiUrl}/${giftId}`, {});
  }
  getWinners(): Observable<Winner[]> {
  return this.http.get<Winner[]>(`${this.apiUrl}/Winner`);
}
}

