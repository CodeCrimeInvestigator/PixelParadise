import { Component } from '@angular/core';
import {UserService} from '../user.service';
import {NgForOf} from '@angular/common';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-test2',
  imports: [
    NgForOf, CommonModule
  ],
  templateUrl: './test2.component.html',
  standalone: true,
  styleUrl: './test2.component.css'
})
export class Test2Component {
  users: any[] = [];
  pageSize: number = 10; // Hardcoded page size
  page: number = 1; // Hardcoded page
  total: number = 0;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getUsers(this.page, this.pageSize).subscribe({
      next: (response) => {
        this.users = response.items;
        this.pageSize = response.pageSize;
        this.page = response.page;
        this.total = response.total;
      },
      error: (err) => {
        console.error('Error fetching users:', err);
      },
    });
  }
}
