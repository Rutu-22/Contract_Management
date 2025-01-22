import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'all-contractors',
    pathMatch: 'full',
  },
  {
    path: 'add-contractor',
    loadComponent: () =>
      import('./components/add-contractor/add-contractor.component').then(
        (mod) => mod.AddContractorComponent
      ),
  },
  {
    path: 'all-contractors',
    loadComponent: () =>
      import('./components/all-contractors/all-contractors.component').then(
        (mod) => mod.AllContractorsComponent
      ),
  },
];
