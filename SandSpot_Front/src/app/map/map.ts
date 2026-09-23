import { setOptions } from './../../../node_modules/@types/leaflet/index.d';
import { Component, AfterViewInit } from '@angular/core';
import * as L from 'leaflet';

@Component({
  imports: [],
  selector: 'app-map',
  styleUrl: './map.css',
  templateUrl: './map.html',
})
export class Map implements AfterViewInit {
  private map: any;

  // Fausses données temporaire (Les spots de beach volley)
  private fakeSpots = [
    {
      id: 1,
      name: 'Plage de la Concurrence',
      lat: 46.155,
      lng: -1.161,
      busy: false,
      alertCount: 0,
    },
    { id: 2, name: 'Plage des Minimes', lat: 46.14086, lng: -1.17114, busy: true, alertCount: 2 },
    { id: 3, name: 'Beach Stadium Aytré', lat: 46.1208, lng: -1.1222, busy: false, alertCount: 0 },
  ];

  constructor() {}

  // On utilise AfterViewInit car il faut que la div #map soit créée dans le HTML AVANT de charger Leaflet
  ngAfterViewInit(): void {
    this.initMap();
    this.addSpots();
  }

  private initMap(): void {
    // 1. Initialisation de la carte (Centrée sur La Rochelle, Zoom 13)
    this.map = L.map('map', {
      zoomControl: false, // On désactive le zoom par défaut pour garder une interface épurée
    }).setView([46.15, -1.15], 13);

    // 2. Ajout du fond de carte OpenStreetMap
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19,
      attribution: '© OpenStreetMap',
    }).addTo(this.map);
  }

  // Construit le pin SVG (couleur + badge selon le statut)
  private buildPinIcon(busy: boolean, alertCount: number): L.DivIcon {
    const fill = busy ? '#FF7A5C' : '#2FB8A6'; // corail si alerte, teal sinon
    const badge = busy
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
      className: 'custom-pin', // important : voir CSS ci-dessous
      iconSize: [26, 32],
      iconAnchor: [13, 32], // pointe du pin sur les coordonnées exactes
      popupAnchor: [0, -28],
    });
  }

  private addSpots(): void {
    // On crée une icône HTML personnalisée pour reprendre ton design de pin 📍
    const spotIcon = L.divIcon({
      html: '📍',
      className: 'custom-pin',
      iconSize: [30, 30],
      iconAnchor: [15, 30], // Pour que la pointe du marqueur soit exactement sur les coordonnées
    });

    // 3. Boucle sur nos fausses données pour ajouter les marqueurs
    this.fakeSpots.forEach((spot) => {
      const icon = this.buildPinIcon(spot.busy, spot.alertCount);
      const marker = L.marker([spot.lat, spot.lng], { icon: icon, title: spot.name }).addTo(
        this.map,
      );

      L.setOptions(marker, {
        idAlerte: spot.id,
        nameAlerte: spot.name,
      });

      marker.on('click', (e) => {
        console.log(e.target.options.idAlerte);
        console.log(e.target.options.nameAlerte);
      });

      // Pour faire un popup au clic d'un marqueur
      // marker.bindPopup(
      //   `<b>${spot.name}</b><br>Statut: ${spot.busy ? '🔥 Alerte en cours' : 'Calme'}`,
      // );
    });
  }
}
