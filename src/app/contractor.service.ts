import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contractor, ContractorResponse } from './models/contractor.model';

@Injectable({
  providedIn: 'root'
})
export class ContractorService {
  private apiUrl = 'https://localhost:7290/api/Contacts';
  constructor(private http: HttpClient) {}

  addContractor(contractor: Contractor): Observable<Contractor> {
    return this.http.post<Contractor>(this.apiUrl, contractor);
  }
  getAllContractors(pageNumber: number, pageSize: number): Observable<ContractorResponse> {
    const url = `${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    return this.http.get<ContractorResponse>(url);
  }
  
  
  updateContractor(id: number, contractorData: Contractor): Observable<Contractor> {
    return this.http.put<Contractor>(`${this.apiUrl}/${id}`, contractorData);
  }
  deleteContractor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  
 
}
