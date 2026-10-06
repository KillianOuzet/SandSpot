import { Component, AfterViewInit, inject } from '@angular/core';
import * as L from 'leaflet';
import 'leaflet.markercluster';
import { ZoneService, Zone } from '../core/zone';

@Component({
  selector: 'app-map',
  styleUrl: './map.css',
  templateUrl: './map.html',
})
export class Map implements AfterViewInit {
  private map: any;
  private lastMarkerSelected: any;
  private markerClusterGroup: any;

  private zoneService = inject(ZoneService);

  ngAfterViewInit(): void {
    this.initMap();
    this.loadZones();
  }

  private initMap(): void {
    this.map = L.map('map', {
      zoomControl: false,
    }).setView([46.15, -1.15], 13); // Centré sur La Rochelle

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19,
      attribution: '© OpenStreetMap',
    }).addTo(this.map);

    // Initialisation du groupe de cluster
    this.markerClusterGroup = L.markerClusterGroup({
      disableClusteringAtZoom: 16, // À partir d'un certain zoom, on sépare tout
      spiderfyOnMaxZoom: true, // Si des points sont au millimètre près, ils s'écartent en toile d'araignée
    });

    this.map.addLayer(this.markerClusterGroup);
  }

  private buildPinIcon(isSelected: boolean, alertCount: number): L.DivIcon {
    const fill = isSelected ? '#FF7A5C' : '#2FB8A6';
    const badge =
      alertCount > 0
        ? `<div style="position:absolute;top:-3px;right:-4px;width:15px;height:15px;
           border-radius:50%;background:#FF7A5C;color:#fff;font-size:9px;
           display:flex;align-items:center;justify-content:center;
           font-family:sans-serif;">${alertCount}</div>`
        : '';

    const svg = `
      <div style="position:relative;width:26px;height:32px;">
        <svg width="26" height="32" viewBox="0 0 24 30">
          <path d="M12 1C6.9 1 3 5 3 10c0 6.5 9 18 9 18s9-11.5 9-18c0-5-3.9-9-9-9z" fill="${fill}"/>
          <g transform="translate(6.5,4.5)" stroke="#fff" stroke-width="1.3" fill="none">
            <circle cx="5.5" cy="5.5" r="4.7"/>
            <path d="M5.5 0.8c1.6 1.6 1.6 3.7 0 4.7s-1.6 3.1 0 4.7"/>
            <path d="M0.8 4c1.6-1 3.7-1 4.7 0s3.1 1 4.7 0"/>
          </g>
        </svg>
        ${badge}
      </div>`;

    return L.divIcon({
      html: svg,
      className: 'custom-pin',
      iconSize: [26, 32],
      iconAnchor: [13, 32], // pointe du pin sur les coordonnées exactes
      popupAnchor: [0, -28],
    });
  }

  private loadZones(): void {
    this.zoneService.fetchZones().subscribe({
      next: (zones: Zone[]) => {
        this.addSpotsToMap(zones);
      },
      error: (err) => console.error('Erreur lors de la récupération des zones', err),
    });
  }

  private addSpotsToMap(zones: Zone[]): void {
    this.markerClusterGroup.clearLayers();

    zones.forEach((zone) => {
      const icon = this.buildPinIcon(false, 0);

      const marker = L.marker([zone.latitude, zone.longitude], {
        icon: icon,
        title: zone.name,
      });

      L.setOptions(marker, { zoneData: zone });

      marker.on('click', (e) => {
        if (this.lastMarkerSelected) {
          this.lastMarkerSelected.setIcon(this.buildPinIcon(false, 0));
        }

        e.target.setIcon(this.buildPinIcon(true, 0));

        const clickedZone = e.target.options.zoneData as Zone;
        console.log('Zone sélectionnée :', clickedZone.name);

        this.map.setView(e.target.getLatLng(), 15);
        this.lastMarkerSelected = e.target;
      });

      this.markerClusterGroup.addLayer(marker);
    });
  }
}
