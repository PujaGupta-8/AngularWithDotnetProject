import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { EmployeeService } from '../employee.service';
import { Subject } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-list-view',
  templateUrl: './list-view.component.html',
  styleUrls: ['./list-view.component.css']
})
export class ListViewComponent {
  employees: any[] = [];
  filteredEmployees: any[] = [];
  paginatedEmployees: any[] = [];
  searchQuery: string = '';
  currentPage: number = 1;
  pageSize: number = 5;
  pages: number[] = [];
  totalCount:number=0;
  private searchSubject: Subject<any> = new Subject<any>();
  constructor(private router: Router ,private employeeService:EmployeeService,private toastr: ToastrService ) {
   
     
  }

  
  ngOnInit() {
    this.fetchEmployees();
    this.updatePagination(this.filteredEmployees.length, this.pageSize);
    
  }

  // fetchEmployees(pageNumber: number = 1, pageSize: number = 5): void {
  //   this.employeeService.getAllEmployee(pageNumber, pageSize).subscribe((data: any) => {
  //     console.log('API Response:', data); // Log the response to verify
  
  //     // Access the employees array from the response
  //     if (Array.isArray(data.employees)) {
  //       this.employees = data.employees; // Assign the employees array
  //       this.filteredEmployees = [...this.employees]; // Clone the array for filtering
  //     } else {
  //       console.error('Employees data is not an array:', data.employees);
  //       this.employees = []; // Fallback to an empty array if data.employees is not an array
  //       this.filteredEmployees = [...this.employees]; // Update filtered employees list
  //     }
  
  //     // Calculate total count and update pagination
  //     const totalCount = data.totalCount ||  this.filteredEmployees.length;
  //     this.updatePagination(totalCount, pageSize);
  //   });
  // }
  
 

  // Pagination logic
  // updatePagination() {
  //   this.pages = [];
  //   const totalPages = Math.ceil(this.filteredEmployees.length / this.pageSize);
  //   for (let i = 1; i <= totalPages; i++) {
  //     this.pages.push(i);
  //   }
  //   this.paginatedEmployees = this.filteredEmployees.slice(
  //     (this.currentPage-1) * this.pageSize,
  //     this.currentPage * this.pageSize
  //   );
  // }
  fetchEmployees(pageNumber: number = 1, pageSize: number = 5): void {
    this.employeeService.getAllEmployee(pageNumber, pageSize).subscribe((data: any) => {
      console.log('API Response:', data); // Log full response to debug
  
      if (data && Array.isArray(data.employees)) {
        this.employees = data.employees; // Assign employees
        this.filteredEmployees = [...this.employees]; // Clone for table display
  
        this.totalCount = data.totalCount || 0; // Total count for pagination
        this.updatePagination(this.totalCount, pageSize);
      } else {
        console.error('Invalid API response:', data);
        this.employees = [];
        this.filteredEmployees = [];
        this.totalCount = 0;
        this.updatePagination(this.totalCount, pageSize);
      }
    }, error => {
      console.error('Error fetching employees:', error);
    });
  }
  
  updatePagination(totalCount: number, pageSize: number): void {
    this.pages = [];
    const totalPages = Math.ceil(totalCount / pageSize); // Calculate total pages
  
    for (let i = 1; i <= totalPages; i++) {
      this.pages.push(i); // Generate page numbers
    }
  }
  
  // updatePagination(totalCount: number, pageSize: number): void {
  //   // Clear previous page numbers
  //   this.pages = [];
    
  //   // Calculate the total number of pages
  //   const totalPages = Math.ceil(totalCount / pageSize);
    
  //   // Ensure current page does not exceed total pages
  //   if (this.currentPage > totalPages) {
  //     this.currentPage = totalPages;
  //   }
  
  //   // Generate the page numbers
  //   this.pages = Array.from({ length: totalPages }, (_, index) => index + 1);
  // }

  changePage(page: number): void {
    if (page < 1 || page > this.pages.length) return;
  
    this.currentPage = page;
    this.fetchEmployees(this.currentPage, this.pageSize);
  }
  
  
  changePageSize(size: number): void {
    this.pageSize = size;
    this.currentPage = 1; // Reset to the first page
    this.fetchEmployees(this.currentPage, this.pageSize);
  }
  
  

  // Search logic
  onSearch() {
    const searchValue = this.searchQuery.trim();
  
    // Check if the search value is not empty
    if (searchValue) {
      // Use helper function to determine if it's a name or email
      const [name, email] = this.extractNameAndEmail(searchValue);
  
      // Call the API to fetch filtered employees
      this.employeeService.getEmployeeByNameEmail(name, email).subscribe({
        next: (response) => {
          this.filteredEmployees = response; // Assign the API response
          this.currentPage = 1; // Reset to first page
        
          // Assuming the response has a property 'TotalCount'
          const totalCount = response.length; // Adjust this based on your API response
          const pageSize = this.pageSize; // Use the current pageSize
          
          // Update pagination with the total count and page size
          this.updatePagination(totalCount, pageSize); 
   
        },
        error: (err) => {
          console.error('Error fetching employees:', err);
          this.filteredEmployees = []; // Reset to an empty array on error
        }
      });
    } else {
      // If search query is empty, reset to the original employee list
      this.filteredEmployees = [...this.employees];
      this.currentPage = 1; // Reset to first page
        
      // Assuming the response has a property 'TotalCount'
      const totalCount = this.employees.length;
      const pageSize = this.pageSize; // Use the current pageSize
      
      // Update pagination with the total count and page size
      this.updatePagination(totalCount, pageSize); 

    }
  }
  
  // Helper function to check if the search query is name or email
  extractNameAndEmail(searchQuery: string): [string, string] {
    let name = '', email = '';
  
    if (searchQuery.includes('@')) {
      // If the search query contains '@', treat it as email
      email = searchQuery;
    } else {
      // Otherwise, treat it as a name
      name = searchQuery;
    }
  
    return [name, email];
  }
  
  

  onAddEmployee() {
    // Add employee logic
    // console.log('Add Employee');
    this.router.navigate(['employeecreate']);
  }

  onEditEmployee(rowId: any) { 
      this.router.navigate(['employeecreate'], { queryParams: { id: rowId } });
    }
     
  

  // onDeleteEmployee(rowId: number) {
  //   this.employees = this.employees.filter(emp => emp.rowId !== rowId);
  //   this.onSearch(); // Reapply the search filter
  // }
  // Delete Employee
  onDeleteEmployee(rowId: number) {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.deleteEmployee(rowId).subscribe({
        next: () => {
          this.showDeleteSuccess();
          this.fetchEmployees(); // Refresh the list
        },
        error: (err) => {
          console.error('Error deleting employee:', err);
          this.toastr.error('Failed to delete employee. Please try again.', 'Error');
        },
      });
    }
  }
  showDeleteSuccess() {
    this.toastr.success('Employee deleted successfully!', 'Success');
  }
  sortKey: string = ''; // Key to track the column to sort
sortDirection: boolean = true; // True for ascending, false for descending

// Sorting logic
sortTable(columnKey: string): void {
  if (this.sortKey === columnKey) {
    this.sortDirection = !this.sortDirection; // Toggle sort direction
  } else {
    this.sortKey = columnKey;
    this.sortDirection = true; // Default to ascending on column change
  }

  this.filteredEmployees.sort((a: any, b: any) => {
    let valueA = a[columnKey];
    let valueB = b[columnKey];

    if (typeof valueA === 'string') {
      valueA = valueA.toLowerCase();
      valueB = valueB.toLowerCase();
    }

    if (valueA < valueB) return this.sortDirection ? -1 : 1;
    if (valueA > valueB) return this.sortDirection ? 1 : -1;
    return 0;
  });
  

    // // Calculate total count after sorting
    //  // Update pagination after sorting
    //  const totalCount = this.filteredEmployees.length;
    //  const pageSize = this.pageSize;
 
    //  // Ensure currentPage is within the range after sorting
    //  if (this.currentPage > Math.ceil(totalCount / pageSize)) {
    //    this.currentPage = Math.max(1, Math.ceil(totalCount / pageSize)); // Set to the last available page if out of range
    //  }
 
    //  this.updatePagination(totalCount, pageSize);
 
}

}
