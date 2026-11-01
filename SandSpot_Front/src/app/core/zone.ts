import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

export interface Zone {
  id: number;
  name: string;
  latitude: number;
  longitude: number;
  address: string;
  city: string;
  postalCode: string;
  distanceInKm: number;
}

@Injectable({
  providedIn: 'root',
})
export class ZoneService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7285/api/zones';

  zones = signal<Zone[]>([]);

  fetchZones(lat?: number, lng?: number) {
    let url = this.apiUrl;

    if (lat !== undefined && lng !== undefined) {
      url += `?latitude=${lat}&longitude=${lng}`;
    }

    return this.http.get<Zone[]>(url).pipe(tap((data) => this.zones.set(data)));
  }
}
