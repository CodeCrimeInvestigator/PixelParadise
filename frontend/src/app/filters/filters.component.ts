import { Component, Output, EventEmitter } from '@angular/core';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  standalone: true,
  imports: [
    FormsModule
  ],
  styleUrls: ['./filters.component.css']
})
export class FiltersComponent {
  filters = {
    name: '',
    description: '',
    priceLowerLimit: null,
    priceUpperLimit: null,
    ownerUsername: '',
    sortBy: ''
  };

  @Output() filterChange = new EventEmitter<any>();

  onFilterChange(): void {
    this.filterChange.emit(this.filters);
  }
}
