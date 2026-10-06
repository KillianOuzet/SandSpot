import { Component, inject, signal, output } from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../core/auth';

@Component({
  selector: 'app-auth',
  imports: [ReactiveFormsModule],
  templateUrl: './auth.html',
  styleUrls: ['./auth.css'],
})
export class Auth {
  private authService = inject(AuthService);

  // permet d'envoyer un signal au parent pour fermer la modale.
  closeModal = output<void>();

  activeTab = signal<'login' | 'register'>('login');

  // Afficher les erreurs potentielle de l'API
  errorMessage = signal<string | null>(null);

  // Formulaire
  authForm = new FormGroup({
    username: new FormControl(''),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
  });

  // Helper pour savoir si on doit afficher une erreur visuelle sur un champ précis
  isInvalid(controlName: string): boolean {
    const control = this.authForm.get(controlName);
    // On retourne true SEULEMENT si le champ est invalide ET qu'il a été manipulé
    return !!control && control.invalid && (control.dirty || control.touched);
  }

  // Optionnel : Helper pour récupérer le type d'erreur exact (pour un message plus précis)
  hasError(controlName: string, errorName: string): boolean {
    const control = this.authForm.get(controlName);
    return !!control && control.hasError(errorName) && (control.dirty || control.touched);
  }

  // Méthode pour changer d'onglet
  setTab(tab: 'login' | 'register') {
    this.activeTab.set(tab);
    this.errorMessage.set(null);
    this.authForm.reset();

    const usernameControl = this.authForm.get('username');

    if (tab === 'register') {
      usernameControl?.setValidators([Validators.required]);
    } else {
      usernameControl?.clearValidators();
    }

    usernameControl?.updateValueAndValidity();
  }

  // Méthode appelée à la validation du formulaire
  onSubmit() {
    if (this.authForm.invalid) return;

    const { username, email, password } = this.authForm.value;

    if (this.activeTab() === 'login') {
      // Logique de CONNEXION
      this.authService.login(email!, password!).subscribe({
        next: () => this.closeModal.emit(),
        error: (err) => this.errorMessage.set('Identifiants incorrects.'),
      });
    } else {
      // Logique d'INSCRIPTION
      this.authService.register(username!, email!, password!).subscribe({
        next: () => this.closeModal.emit(),
        error: (err) => this.errorMessage.set(err.error?.Error || "Erreur lors de l'inscription."),
      });
    }
  }
}
