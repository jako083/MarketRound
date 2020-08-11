import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { MarketRound } from '../models/MarketRound'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MarketService {
  // API Address
  MarketUrl:string = 'https://jsonplaceholder.typicode.com/users/1/todos';
  // Limit amount of posts
  marketLimit = '?_limit=6';

  constructor(private http:HttpClient) { }

  getMarket():Observable<MarketRound[]> {
    return this.http.get<MarketRound[]>(`${this.MarketUrl} ${this.marketLimit}`);
  }
}
