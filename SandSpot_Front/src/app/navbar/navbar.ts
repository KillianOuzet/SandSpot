import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../core/auth';
import { Auth } from '../auth/auth';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive, Auth],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css'],
})
export class Navbar {
  authService = inject(AuthService);

  isAuthModalOpen = signal(false);

  openAuthModal() {
    this.isAuthModalOpen.set(true);
  }

  closeAuthModal() {
    this.isAuthModalOpen.set(false);
  }

  logout() {
    this.authService.logout();
  }
}
