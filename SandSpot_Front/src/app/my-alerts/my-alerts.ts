import { Component, signal, computed } from '@angular/core';
import { AlertCard } from '../alert-card/alert-card';
import { Alert } from '../core/models/alert.model';

@Component({
  imports: [AlertCard],
  selector: 'app-my-alerts',
  styleUrl: './my-alerts.css',
  templateUrl: './my-alerts.html',
})
export class MyAlerts {
  // L'état de l'onglet actif stocké dans un Signal
  activeTab = signal<'created' | 'joined'>('created');

  // La liste brute stockée dans un Signal
  allAlerts = signal<Alert[]>([
    {
      id: 1,
      title: 'Plage des Minimes',
      date: "Aujourd'hui, 18h",
      playersCount: '2/4 joueurs',
      details: 'Débutant, Intermédiaire',
      status: 'À venir',
      isCreatedByMe: true,
    },
    {
      id: 2,
      title: 'Plage de la Concurrence',
      date: 'Hier, 10h',
      playersCount: '4/4 joueurs',
      details: 'Tous niveaux',
      status: 'Terminée',
      isCreatedByMe: true,
    },
    {
      id: 3,
      title: 'Beach stadium Aytré',
      date: 'Samedi, 15h',
      playersCount: '3/6 joueurs',
      details: 'Organisé par Lucas',
      status: 'À venir',
      isCreatedByMe: false,
    },
  ]);

  // Ce signal se recalcule automatiquement dès que activeTab() change
  filteredAlerts = computed(() => {
    const isCreated = this.activeTab() === 'created';
    return this.allAlerts().filter((a) => a.isCreatedByMe === isCreated);
  });

  // Méthode pour changer la valeur du signal
  setTab(tab: 'created' | 'joined') {
    this.activeTab.set(tab);
  }
}
