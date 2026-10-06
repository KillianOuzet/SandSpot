export interface Alert {
  id: number;
  title: string;
  date: string;
  playersCount: string;
  details: string;
  status: 'À venir' | 'Complet' | 'Terminée';
  isCreatedByMe: boolean;
}
