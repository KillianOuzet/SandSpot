import {Component, EventEmitter, Output} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  imports: [
    FormsModule
  ],
  selector: 'app-filters',
  styleUrl: './filters.css',
  templateUrl: './filters.html',
})
export class Filters {
  distance: number = 12;
  selectedPeriod: string = "Aujourd'hui";
  selectedEquipment: Set<string> = new Set();

  periods: string[] = ["Aujourd'hui", "Demain", "Semaine"];
  availableEquipment: string[] = ["Ballon", "Filet"];

  // Déclencheurs envoyés au parent (la carte)
  @Output() distanceChange = new EventEmitter<number>();
  @Output() recenter = new EventEmitter<void>();

  onDistanceSliderChange(value: number) {
    this.distance = value;
    this.distanceChange.emit(this.distance);
  }

  setPeriod(period: string) {
    this.selectedPeriod = period;
  }

  toggleEquipment(item: string) {
    if (this.selectedEquipment.has(item)) {
      this.selectedEquipment.delete(item);
    } else {
      this.selectedEquipment.add(item);
    }
  }

  onRecenterClick() {
    this.recenter.emit();
  }
}
