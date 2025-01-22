import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators, FormsModule } from '@angular/forms';
import { Contractor } from '../../models/contractor.model';
import { HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ContractorService } from '../../contractor.service';
import { RouterModule } from '@angular/router';
import { customEmailValidator } from '../../validators/email.validator';

@Component({
  selector: 'app-all-contractors',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule,FormsModule, HttpClientModule],
  templateUrl: './all-contractors.component.html',
  styleUrls: ['./all-contractors.component.scss'],
  providers: [ContractorService],
})
export class AllContractorsComponent {
  Contractor: Contractor[] = [];
  contractorForm: FormGroup;
  selectedContractor: Contractor | null = null;
  loading = false;
  currentPage = 1;
  totalPages = 1;
  pageSize = 25; 
  totalItems = 0;
  error: string | null = null;
  showModal: boolean = false;
  successMessage: string | null = null;
  timeoutId: any;


  contractorService = inject(ContractorService);

  constructor() {
    this.contractorForm = new FormGroup({
      firstName: new FormControl('', [ Validators.maxLength(50)]),
      lastName: new FormControl('', [ Validators.maxLength(50)]),
      email: new FormControl('', [ customEmailValidator()]),
      mobile: new FormControl('', [ Validators.pattern('^[0-9]{10}$')]),
      gender: new FormControl('', [Validators.required]),
      status: new FormControl('active', []),
    });
  }

  ngOnInit(): void {
    this.loadContractors(this.currentPage, this.pageSize);
  }

  onUpdate(): void {
  if (this.contractorForm.valid && this.selectedContractor) {
    this.loading = true;
    const updatedContractor: Contractor = {
      ...this.selectedContractor,
      ...this.contractorForm.getRawValue(),
    };

    this.contractorService.updateContractor(this.selectedContractor.id, updatedContractor)
      .subscribe(
        (response: Contractor) => {
          this.loading = false;
          this.loadContractors();  
          this.closeModal(); 
          this.showSuccessMessage(`Contractor ${updatedContractor?.firstName} ${updatedContractor?.lastName} updated successfully!`);
        
                  
          setTimeout(() => (this.successMessage = null), 3000);
        },
        (error: HttpErrorResponse) => {
          this.loading = false;
          this.error = error.message;
        }
      );
  }
}
  
showSuccessMessage(message: string): void {
  this.successMessage = message;
  setTimeout(() => {
    this.successMessage = null;
  }, 5000); 
}

  
  closeModal(): void {
    this.showModal = false; 
    this.selectedContractor = null;
  }

  onEdit(contractor: Contractor): void {
    this.selectedContractor = contractor; 
    this.contractorForm.patchValue({
      firstName: contractor.firstName,
      lastName: contractor.lastName,
      email: contractor.email,
      mobile: contractor.mobile,
      gender: contractor.gender, 
      status: contractor.status?.toLowerCase() || 'active',  
    });

    this.contractorForm.get('firstName')?.disable();
  this.contractorForm.get('lastName')?.disable();
  this.contractorForm.get('email')?.disable();
  this.contractorForm.get('mobile')?.enable();
  this.contractorForm.get('gender')?.disable();
  this.contractorForm.get('status')?.enable();
    this.showModal = true; 
  }

  onDelete(contractor: Contractor): void {
    if (confirm(`Are you sure you want to delete contractor ${contractor.firstName} ${contractor.lastName}?`)) {
      this.loading = true;
      this.contractorService.deleteContractor(contractor.id).subscribe(
        () => {
          this.Contractor = this.Contractor.filter(c => c.id !== contractor.id);
          this.loading = false;
  
          
          this.showSuccessMessage(
            `Contractor ${contractor?.firstName} ${contractor?.lastName} deleted successfully!`
          );         
          setTimeout(() => (this.successMessage = null), 3000);
        },
        (error: HttpErrorResponse) => {
          this.error = error.message;
          this.loading = false;
        }
      );
    }
  }
  
  


  loadContractors(pageNumber: number = 1, pageSize: number = this.pageSize): void {
    this.loading = true;
    this.contractorService.getAllContractors(pageNumber, pageSize).subscribe(
      (response) => {
        this.Contractor = response.data; 
      this.totalItems = response.totalItems || 0;
      this.totalPages = Math.ceil(this.totalItems / this.pageSize); 
      this.currentPage = pageNumber; 
      this.loading = false;
     
      },
      (error: HttpErrorResponse) => {
        this.error = error.message;
        this.loading = false;
      }
    );
  }
  
  onPageSizeChange(event: any): void {
    this.pageSize = parseInt(event.target.value, 10); 
    this.currentPage = 1; 
    this.loadContractors(this.currentPage, this.pageSize);
  }
  
  
  loadNextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.loadContractors(this.currentPage + 1, this.pageSize);
    }
  }
  
  loadPreviousPage(): void {
    if (this.currentPage > 1) {
      this.loadContractors(this.currentPage - 1, this.pageSize);
    }
  }
  
}

