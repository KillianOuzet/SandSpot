import { Component, input } from '@angular/core';
import { Alert } from '../core/models/alert.model';

@Component({
  selector: 'app-alert-card',
  imports: [],
  templateUrl: './alert-card.html',
  styleUrls: ['./alert-card.css'],
})
export class AlertCard {
  // Déclaration du Signal d'entrée.
  alert = input.required<Alert>();
}
