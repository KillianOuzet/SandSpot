import { Component } from '@angular/core';
import { Filters } from '../filters/filters';
import { Map } from '../map/map';
import { Sidebar } from '../sidebar/sidebar';

@Component({
  imports: [Filters, Map, Sidebar],
  selector: 'app-home',
  styleUrl: './home.css',
  templateUrl: './home.html',
})
export class Home {}
