import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';

export interface Accommodation {
  id: string;
  name: string;
  description: string;
  price: number;
  ownerId: string;
  coverImageUrl: string;
  images: string[];
}

interface AccommodationResponse {
  items: Accommodation[];
  pageSize: number;
  page: number;
  total: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AccommodationsService {
  private apiUrl = 'http://localhost:5000/api/accommodation';
  constructor(private http: HttpClient) {}

  getAccommodations(page: number, pageSize: number): Observable<AccommodationResponse> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<AccommodationResponse>(this.apiUrl, { params });
  }

  getFilteredAccommodations(
    page: number,
    pageSize: number,
    filters: any
  ): Observable<AccommodationResponse> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (filters.name) {
      params = params.set('Name', filters.name);
    }
    if (filters.description) {
      params = params.set('Description', filters.description);
    }
    if (filters.priceLowerLimit != null) {
      params = params.set('PriceLowerLimit', filters.priceLowerLimit.toString());
    }
    if (filters.priceUpperLimit != null) {
      params = params.set('PriceUpperLimit', filters.priceUpperLimit.toString());
    }
    if (filters.ownerUsername) {
      params = params.set('OwnerUsername', filters.ownerUsername);
    }
    if (filters.sortBy) {
      params = params.set('SortBy', filters.sortBy);
    }

    return this.http.get<AccommodationResponse>(this.apiUrl, {params});
  }
 /* searchAccommodationsByName(page: number, pageSize: number, name: string): Observable<AccommodationResponse> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('Name', name.toString());
    return this.http.get<AccommodationResponse>(this.apiUrl, { params });
  }*/
}
