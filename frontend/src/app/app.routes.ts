import { Routes } from '@angular/router';
import {HomeComponent} from './home/home.component';
import {AccommodationsComponent} from './accommodations/accommodations.component';
import {AboutComponent} from './about/about.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'accommodations', component: AccommodationsComponent },
  { path: 'about', component: AboutComponent }
];
