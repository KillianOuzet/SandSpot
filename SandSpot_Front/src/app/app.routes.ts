import { Routes } from '@angular/router';
import { Home } from './home/home';
import { MyAlerts } from './my-alerts/my-alerts';

export const routes: Routes = [
  // Le path racine
  { path: '', component: Home },
  
  // Le path pour mes alertes
  { path: 'mes-alertes', component: MyAlerts },
  
  // Règle de sécurité : si l'utilisateur tape n'importe quoi dans l'URL, on le ramène à l'accueil
  { path: '**', redirectTo: '' } 
];