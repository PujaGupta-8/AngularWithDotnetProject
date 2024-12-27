import { CommonModule } from '@angular/common';
import { Component, OnInit, Pipe } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EmployeeService } from '../employee.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-employee-create',
  templateUrl: './employee-create.component.html',
  styleUrls: ['./employee-create.component.css'],

})

export class EmployeeCreateComponent implements OnInit {

  employeeForm!: FormGroup;
  countries: any[] = [];
  genders: any[] = [];
  states: any[] = [];
  cities: any[] = [];
  rowId: any;
  maxDate!: string;
  todayDate!: string;
  minDateForJoinee!: string; 
  showFileInput: boolean = false;
  selectedFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;
  employees: any;
  filteredEmployees: any[] = [];
  paginatedEmployees: any[] = [];
  currentPage: number = 1;
  pageSize: number = 5;
  // employeeId: number | null = null;
  employeeDetails: any = {}; // Initializing employeeDetails to an empty object

  constructor(private route: ActivatedRoute,
    private fb: FormBuilder,
    private employeeService: EmployeeService, private router: Router,private toastr: ToastrService) {
    this.setMaxDate();
    this.setTodayDate();
    this.getAllCountries();
    this.getAllGender();

  }
  ngOnInit(): void {

    console.log('row', this.rowId);

    // this.route.queryParams.subscribe((params: any) => {
    //   this.rowId = params['id'] ? +params['id'] : null;
    //   if (this.rowId) {
    //     this.fetchEmployees(this.rowId);
    //   }

    // });
    this.route.queryParams.subscribe((params: any) => {
      // Retrieve the 'id' and 'pageNumber' query parameters from the URL
      this.rowId = params['id'] ? +params['id'] : null;
      const pageNumber = params['pageNumber'] ? +params['pageNumber'] : 1; // Default to 1 if not provided
  
      // If the 'rowId' is found, fetch employee details
      if (this.rowId) {
        this.fetchEmployees(pageNumber, 5, this.rowId); // Fetch employees for the given page number
      }
    });

    this.employeeForm = this.fb.group({
      rowId: [0],
      FirstName: ['', [Validators.required, Validators.pattern('^[A-Za-z ]+$')]],
      lastName: ['', [Validators.required, Validators.pattern('^[A-Za-z ]+$')]],
      //employeeCode:['',[Validators.required]],
      countryId: [0, [Validators.required]],
      stateId: [0, [Validators.required]],
      cityId: [0, [Validators.required]],
      emailAddress: ['', [Validators.required, Validators.email]],
      mobileNumber: ['', [
        Validators.required,
        Validators.maxLength(10),
        Validators.pattern('^[0-9]{10}$')  // This pattern ensures exactly 10 digits
      ]],
      panNumber: ['', [Validators.required, Validators.maxLength(10), Validators.pattern('^[A-Za-z0-9]{10}$')]],
     passportNumber: ['', [Validators.required,Validators.maxLength(10),  Validators.pattern('^[A-Za-z0-9]{6,10}$')]],
      profileImage: [''],
      gender: [0, [Validators.required]],
      isActive: [true],
      isDeleted: [false],
      dateOfBirth: ['', [Validators.required]],
      dateOfJoinee: ['', [Validators.required]],
      employeeCode: [''],
    });


    // Call validateAge whenever dateOfBirth or dateOfJoinee changes
    this.employeeForm.get('dateOfBirth')?.valueChanges.subscribe(() => {
      this.onDateOfBirthChange();
      this.validateAge();
    });
    this.employeeForm.get('dateOfJoinee')?.valueChanges.subscribe(() => {
      this.validateAge();
    });
  }


  getAllCountries() {
    this.employeeService.getAllCountry().subscribe((data: any) => {
      this.countries = data;
      console.log(this.countries)
    });
  }
  getAllGender() {
    this.employeeService.getAllGender().subscribe((data: any) => {
      this.genders = data;
      console.log(this.countries)
    });
  }
 
  restrictNonNumeric(event: KeyboardEvent): void {
  const inputValue = event.key;
  const regex = /^[0-9]$/; // This regex allows only numeric values (0-9)

  if (!regex.test(inputValue)) {
    event.preventDefault();  // Prevent any non-numeric characters from being typed
  }
}
 
  
  setTodayDate() {
    const today = new Date();
    this.todayDate = today.toISOString().split('T')[0];  // Set today's date
  }
  
  setMaxDate() {
    const today = new Date();
    today.setFullYear(today.getFullYear() - 18);  // Subtract 18 years for max DOB
    this.maxDate = today.toISOString().split('T')[0]; // Set max date for DOB
  }
  
  onDateOfBirthChange() {
    const dob = this.employeeForm.get('dateOfBirth')?.value;
    if (dob) {
      const dobDate = new Date(dob);
      dobDate.setFullYear(dobDate.getFullYear() + 18);  // Calculate DOJ min date by adding 18 years to DOB
      this.minDateForJoinee = dobDate.toISOString().split('T')[0];  // Set the min DOJ date
    }
  }
  

  validateAge() {
    const dob = this.employeeForm.get('dateOfBirth')?.value;
    const doj = this.employeeForm.get('dateOfJoinee')?.value;
  
    if (dob && doj) {
      const dobDate = new Date(dob);
      const dojDate = new Date(doj);
  
      // Calculate the difference in years
      let age = dojDate.getFullYear() - dobDate.getFullYear();
      const m = dojDate.getMonth() - dobDate.getMonth();
      
      // Adjust if the DOB hasn't yet occurred this year
      if (m < 0 || (m === 0 && dojDate.getDate() < dobDate.getDate())) {
        age--;
      }
  
      // Check if age is less than 18
      if (age < 18) {
        this.employeeForm.get('dateOfJoinee')?.setErrors({ ageInvalid: true });
      } else {
        // Remove the error if valid
        if (this.employeeForm.get('dateOfJoinee')?.hasError('ageInvalid')) {
          this.employeeForm.get('dateOfJoinee')?.updateValueAndValidity();
        }
      }
    }
  }  

  // fetchEmployees(rowId: number): void {
  //   this.employeeService.getAllEmployee().subscribe((data: any) => {
  //     const employee = data.find((emp: any) => emp.rowId === rowId);

  //     if (employee) {
  //       console.log(employee)
  //       this.getAllStates(employee.countryId);
  //       this.getAllCities(employee.stateId);

  //       this.employeeForm.patchValue({
  //         rowId: employee.rowId,
  //         FirstName: employee.firstName,
  //         lastName: employee.lastName,
  //         countryId: employee.countryId,
  //         stateId: employee.stateId,
  //         cityId: employee.cityId,
  //         emailAddress: employee.emailAddress,
  //         mobileNumber: employee.mobileNumber,
  //         panNumber: employee.panNumber,
  //         passportNumber: employee.passportNumber,
  //         dateOfBirth: employee.dateOfBirth ? new Date(employee.dateOfBirth).toISOString().split('T')[0] : null,
  //         dateOfJoinee: employee.dateOfJoinee ? new Date(employee.dateOfJoinee).toISOString().split('T')[0] : null,

  //         gender: employee.gender,
  //         isActive: employee.isActive,
  //         isDeleted: employee.isDeleted,
  //         profileImage: '',
  //         employeeCode: employee.employeeCode

  //       });

  //     } else {
  //       console.error('Employee not found for the given ID:', rowId);
  //     }
  //   });

  // }
  fetchEmployees(pageNumber: number = 1, pageSize: number = 5, rowId: number): void {
    // Fetch employees based on pagination parameters
    this.employeeService.getAllEmployee(pageNumber, pageSize).subscribe((data: any) => {
      console.log('API Response:', data); // Log the response for debugging
  
      // Check if the response contains the employees array
      if (data && Array.isArray(data.employees)) {
        // Find the employee with the specified rowId
        const employee = data.employees.find((emp: any) => emp.rowId === rowId);
  
        if (employee) {
          console.log('Found Employee:', employee);
          this.getAllStates(employee.countryId);
          this.getAllCities(employee.stateId);
  
          // Patch the employee data to the form
          this.employeeForm.patchValue({
            rowId: employee.rowId,
            FirstName: employee.firstName,
            lastName: employee.lastName,
            countryId: employee.countryId,
            stateId: employee.stateId,
            cityId: employee.cityId,
            emailAddress: employee.emailAddress,
            mobileNumber: employee.mobileNumber,
            panNumber: employee.panNumber,
            passportNumber: employee.passportNumber,
            dateOfBirth: employee.dateOfBirth ? new Date(employee.dateOfBirth).toISOString().split('T')[0] : null,
            dateOfJoinee: employee.dateOfJoinee ? new Date(employee.dateOfJoinee).toISOString().split('T')[0] : null,
            gender: employee.gender,
            isActive: employee.isActive,
            isDeleted: employee.isDeleted,
            profileImage: '',
            employeeCode: employee.employeeCode
          });
        } else {
          console.error('Employee not found for the given ID:', rowId);
        }
      } else {
        console.error('Employees data is not an array:', data);
      }
    });
  }
  
  
  


  onCountryChange(event: Event): void {
    const selectedCountryId = (event.target as HTMLSelectElement).value;
    if (selectedCountryId) {
      this.getAllStates(Number(selectedCountryId));
    }
  }


  getAllStates(countryId: number) {
    this.employeeService.getAllStates(countryId)
      .subscribe((data: any) => {
        this.states = data;
      });
  }

  onStateChange(event: Event): void {
    const selectedStateId = (event.target as HTMLSelectElement).value;
    if (selectedStateId) {
      this.getAllCities(Number(selectedStateId));
    }
  }

  getAllCities(stateId: number) {
    this.employeeService.getAllCities(stateId)
      .subscribe((data: any) => {
        this.cities = data;
      });
  }

  onSubmit() {
    // debugger;
    if (this.employeeForm.valid) {
      // Extract the form data
      const employeeDetails = this.employeeForm.value;

      // Prepare a FormData object to handle file upload
      const formData = new FormData();

      // Append all form values to FormData
      Object.keys(this.employeeForm.value).forEach((key) => {
        // debugger
        if (key !== 'profileImage') {

          if (key === 'dateOfBirth' || key === 'dateOfJoinee') {
            const dateValue = this.employeeForm.value[key];
            formData.append(key, dateValue ? dateValue : ''); // Date already in yyyy-MM-dd
          } else {

            formData.append(key, this.employeeForm.value[key]);
          }
        }
        if (
          this.employeeForm.get('profileImage')?.value == undefined ||
          this.employeeForm.get('profileImage')?.value == "" ||
          this.employeeForm.get('profileImage')?.value == null) {

          formData.append('profileImage', "");
        }
        else if (this.selectedFile) {
          formData.append('profileImage', this.selectedFile);
        }

      });
      console.log('Form Data:', formData);

      if (employeeDetails.rowId && employeeDetails.rowId > 0) {
        // Update logic
        console.log('RowId:', employeeDetails.rowId);
        this.employeeService.updateEmployeeDetails(formData).subscribe({
          next: (response) => {
            console.log('Employee updated successfully', response);
            this.toastr.success('Employee updated successfully!', 'Success');
            this.router.navigate(['/employee-list']); // Redirect to list page
          },
          error: (err) => {
            console.error('Error updating employee', err);
            this.toastr.error('Failed to update employee. Please try again.', 'Error');
          },
        });
      } else {
        // Add logic
        this.employeeService.addEmployeeDetails(formData).subscribe({
          next: (response) => {
            console.log('Employee added successfully', response);
            this.toastr.success('Employee added successfully!', 'Success');

            this.router.navigate(['/employee-list']); // Redirect to list page
          },
          error: (err) => {
            console.error('Error adding employee', err);
          this.toastr.error('Failed to add employee. Please try again.', 'Error');

          },
        });
      }
    } else {
      console.error('Form is invalid');
      this.toastr.error('Please fill out the form correctly.', 'Invalid Form');
    }
  }

  onFileSelected(event: Event): void {
    const fileInput = event.target as HTMLInputElement;

    if (fileInput.files && fileInput.files.length > 0) {
      this.selectedFile = fileInput.files[0];
      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        if (e.target && e.target.result) {
          this.imagePreview = e.target.result;
          this.showFileInput = false;
        }
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  // // Helper method to check if a date is valid
  // isValidDate(dateString: string): boolean {
  //   const date = new Date(dateString);

  //   // Check if the date is a valid date object and within the valid SQL range
  //   const minDate = new Date(1753, 0, 1); // 1st Jan 1753
  //   const maxDate = new Date(9999, 11, 31); // 31st Dec 9999

  //   return date instanceof Date && !isNaN(date.getTime()) && date >= minDate && date <= maxDate;
  // }

  getEmployeeDetails(id: number) {
    this.employeeService.getEmployeeById(id).subscribe({
      next: (data) => {
        this.employeeDetails = data;  // Populate employee data in the form
        this.employeeForm.patchValue({
          rowId: data.rowId,  // Ensure rowId is bound to form control
          firstName: data.firstName,
          lastName: data.lastName,
          countryId: data.countryId,
          stateId: data.stateId,
          cityId: data.cityId,
          emailAddress: data.emailAddress,
          mobileNumber: data.mobileNumber,
          panNumber: data.panNumber,
          passportNumber: data.passportNumber,
          profileImage: null,
          gender: data.gender,
          isActive: data.isActive,
          dateOfBirth: data.dateOfBirth ? new Date(data.dateOfBirth).toISOString().split('T')[0] : null,
          dateOfJoinee: data.dateOfJoinee ? new Date(data.dateOfJoinee).toISOString().split('T')[0] : null,

        });
        console.log('Employee data fetched for update:', this.employeeDetails);
      },
      error: (err) => {
        console.error('Error fetching employee data:', err);
      },
    });
  }
  formatDate(date: any): string {
    if (!date) return '';
    let d = new Date(date);
    return d.toISOString().split('T')[0];  // Format as YYYY-MM-DD
  }


  onInputUppercase(event: Event, formControlName: string): void {
    const inputElement = event.target as HTMLInputElement;
    const uppercasedValue = inputElement.value.toUpperCase();
    inputElement.value = uppercasedValue; // Update the DOM value
    this.employeeForm.get(formControlName)?.setValue(uppercasedValue, { emitEvent: false }); // Update the FormControl value
  }

  


}
