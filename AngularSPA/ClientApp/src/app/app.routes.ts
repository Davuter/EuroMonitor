import { Routes, RouterModule } from "@angular/router";


import { CatalogComponent } from './catalog/catalog.component';
import { SubscriptionsComponent } from './subscriptions/subscriptions.component';


export const routes: Routes = [
  { path: '', redirectTo: 'catalog', pathMatch: 'full' },
  { path: 'catalog', component: CatalogComponent },
  { path: 'subscriptions', component: SubscriptionsComponent }
];

export const routing = RouterModule.forRoot(routes);
