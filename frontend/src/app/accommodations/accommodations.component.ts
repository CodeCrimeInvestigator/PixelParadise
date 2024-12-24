import {Component, OnInit} from '@angular/core';
import {Accommodation, AccommodationsService} from '../accommodations.service';
import {NgForOf} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {FiltersComponent} from '../filters/filters.component';

@Component({
  selector: 'app-accommodations',
  imports: [NgForOf, FormsModule, FiltersComponent],
  templateUrl: './accommodations.component.html',
  standalone: true,
  styleUrl: './accommodations.component.css'
})
export class AccommodationsComponent implements OnInit {
  accommodations: Accommodation[] = [];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;
  searchQuery: string = '';
  filters = {
    name: '',
    description: '',
    priceLowerLimit: null,
    priceUpperLimit: null,
    ownerUsername: '',
    sortBy: ''
  };

  constructor(private accommodationsService: AccommodationsService) {}

  ngOnInit(): void {
    this.loadAccommodations(this.currentPage, this.pageSize);
  }

  /*searchAccommodations(page: number, pageSize: number): void {
    if (this.searchQuery.trim()) {
      this.accommodationsService.searchAccommodationsByName(page, pageSize, this.searchQuery).subscribe((response) => {
        this.accommodations = response.items;
        this.totalItems = response.total;
      });
    } else {
      this.loadAccommodations(this.currentPage, this.pageSize); // Load all accommodations if search is empty
    }
  }
*/
/*  loadAccommodations(page: number, pageSize: number): void {
    this.accommodationsService.getAccommodations(page, pageSize).subscribe((response) => {
      this.accommodations = response.items;
      this.totalItems = response.total;
      this.currentPage = response.page;
    });
  }*/

  loadAccommodations(page: number, pageSize: number): void {
    this.accommodationsService
      .getFilteredAccommodations(page, pageSize, this.filters)
      .subscribe((response) => {
        this.accommodations = response.items;
        this.totalItems = response.total;
        this.currentPage = response.page;
      });
  }

  applyFilters(filters: any): void {
    this.filters = filters;
    this.loadAccommodations(this.currentPage, this.pageSize);
  }
}
