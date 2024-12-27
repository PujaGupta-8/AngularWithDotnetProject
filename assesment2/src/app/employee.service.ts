import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})

export class EmployeeService {
  private apiUrl = 'https://localhost:7244/api/Employee'
  constructor(private http: HttpClient) { }

 
  getAllEmployee(pageNumber: number = 1, pageSize: number = 5): Observable<any> {
    const url = `${this.apiUrl}/EmployeeList?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    return this.http.get<any>(url);
  }
  
  getEmployeeById(Id:number):Observable<any>
  {
    const params = {
      Id: Id ? Id.toString() : '',
    };
    return this.http.get(`${this.apiUrl}/GetEmployeeById`,{params})
  }

  getEmployeeByNameEmail(Name: string, email: string): Observable<any> {
    const params = { Name: Name, email: email }; // Include query parameters
    return this.http.get(`${this.apiUrl}/EmployeeListByNameEmail`, { params });
  }
  
  getAllCountry(): Observable<any> {
    return this.http.get(`${this.apiUrl}/CountryList`);
  }
  getAllGender(): Observable<any> {
    return this.http.get(`${this.apiUrl}/GenderList`);
  }
  getAllStates(Id: number): Observable<any> {
    const params = {
      Id: Id ? Id.toString() : '',
    };
    return this.http.get(`${this.apiUrl}/StateListByCountry`, { params });
  }

  getAllCities(Id: number): Observable<any> {
    const params = {
      Id: Id ? Id.toString() : '',
    };
    return this.http.get(`${this.apiUrl}/CityListByState`, { params });
  }

  addEmployeeDetails(employeeDetails: any): Observable<any> {
    
    return this.http.post(`${this.apiUrl}/AddEmployeeDetails`, employeeDetails);
  }
  updateEmployeeDetails(employeeDetails: any): Observable<any> {
    // Define the payload you want to send in the body of the PUT request
    // const params = {
    //   rowId: employeeDetails.rowId,
    //   employeeCode: employeeDetails.employeeCode,
    //   firstName: employeeDetails.firstName,
    //   lastName: employeeDetails.lastName,
    //   countryId: employeeDetails.countryId,
    //   stateId: employeeDetails.stateId,
    //   cityId: employeeDetails.cityId,
    //   emailAddress: employeeDetails.emailAddress,
    //   mobileNumber: employeeDetails.mobileNumber,
    //   panNumber: employeeDetails.panNumber,
    //   passportNumber: employeeDetails.passportNumber,
    //   profileImage: employeeDetails.profileImage,
    //   gender: employeeDetails.gender,
    //   isActive: employeeDetails.isActive,
    //   dateOfBirth: employeeDetails.dateOfBirth,
    //   dateOfJoinee: employeeDetails.dateOfJoinee
    // };
    //employeeDetails.dateOfBirth=employeeDetails.dateOfBirth.toISOString();

    return this.http.put<any>(`${this.apiUrl}/UpdateEmployeeDetails`, employeeDetails )  // Send PUT request to update employee details
  }

  // Delete employee by ID
  deleteEmployee(rowId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteEmployeeDetails?id=${rowId}`);
  }
}



