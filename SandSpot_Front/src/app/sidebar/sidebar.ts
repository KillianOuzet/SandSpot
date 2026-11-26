import { Component } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-sidebar',
  styleUrl: './sidebar.css',
  templateUrl: './sidebar.html',
})
export class Sidebar {
  alertTitle: string = "Plage des Minimes";
  date: string = "Aujourd'hui";
  hour: string = "18";
  playerCount: number = 4;
}
