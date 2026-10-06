import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

interface AuthResponse {
  token: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);

  private apiUrl = 'https://localhost:7285/api/auth';

  isLoggedIn = signal<boolean>(this.hasToken());

  // Vérifie la présence du JWT dans le stockage local
  private hasToken(): boolean {
    return !!localStorage.getItem('token');
  }

  // Appel HTTP pour l'inscription
  register(username: string, email: string, password: string) {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/register`, { username, email, password })
      .pipe(
        // "tap" permet d'exécuter une action secondaire (sauvegarder le token) quand la requête réussit
        tap((response) => this.handleAuthSuccess(response.token)),
      );
  }

  // Appel HTTP pour la connexion
  login(email: string, password: string) {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, { email, password })
      .pipe(tap((response) => this.handleAuthSuccess(response.token)));
  }

  // Méthode utilitaire appelée après un succès
  private handleAuthSuccess(token: string) {
    localStorage.setItem('token', token); // On sauvegarde le JWT
    this.isLoggedIn.set(true);
  }

  // Déconnexion
  logout() {
    localStorage.removeItem('token');
    this.isLoggedIn.set(false);
  }
}
