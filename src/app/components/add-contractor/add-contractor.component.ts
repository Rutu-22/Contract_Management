import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ContractorService } from '../../contractor.service';
import { customEmailValidator } from '../../validators/email.validator';
import { Contractor } from '../../models/contractor.model';
import { ModalComponent } from '../../modal/modal.component';
import { catchError, timeout } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-add-contractor',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule, CommonModule, HttpClientModule, ModalComponent],
  templateUrl: './add-contractor.component.html',
  styleUrls: ['./add-contractor.component.scss'],
  providers: [ContractorService]
})
export class AddContractorComponent implements OnInit {
  contractorForm: FormGroup;
  @ViewChild(ModalComponent) modal!: ModalComponent;
  message: string = '';
  isSubmitting: boolean = false;
  messageType: string | null = null;
  showModal: boolean = false;
  constructor(
    private fb: FormBuilder,
    private contractorService: ContractorService,
    private router: Router
  ) {
    this.contractorForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50),Validators.pattern('^[A-Za-z ]*$')]],
      lastName: ['', [Validators.required, Validators.maxLength(50),Validators.pattern('^[A-Za-z ]*$')]],
      email: ['', [Validators.required, customEmailValidator()]],
      mobile: ['', [Validators.required, Validators.pattern('[0-9]{10}')]],
      gender: ['', [Validators.required]],
      status: ['active', [Validators.required]]
    });
  }

  ngOnInit(): void {}

  get formControls() {
    return this.contractorForm.controls;
  }
  handleModalClose() {
    console.log('Modal closed');
  }
  
  onSubmit(): void {
    if (this.contractorForm.invalid) {
      return;
    }
    this.isSubmitting = true;
    const contractorData: Contractor = this.contractorForm.value;
  
    this.contractorService.addContractor(contractorData).subscribe(
      (response) => {
        this.message = 'Contractor added successfully!';
        this.messageType = 'success'; 
        this.showModal = true;
        setTimeout(() => {
          this.message = ''; 
        }, 3000);
        this.contractorForm.reset({
          firstName: '',
          lastName: '',
          email: '',
          mobile: '',
          gender: '',
          status: 'active'
        }); 
      },
        (error) => {
          if (error.error && error.error.message) {
            this.message = error.error.message; 
          } else if (error.error && error.error.error) {
            this.message = error.error.error; 
          } else {
            this.message = 'Error adding contractor. Please try again later.'; 
          }
          this.messageType = 'error'; 
          setTimeout(() => {
            this.message = ''; 
          }, 5000);
        },
      () => {
        this.isSubmitting = false;
      }
    );
  }

  onModalConfirm(): void {
    this.showModal = false;
    this.contractorForm.reset({
      firstName: '',
      lastName: '',
      email: '',
      mobile: '',
      gender: '',
      status: 'active'
    }); 
  }
  
  onModalCancel(): void {
    this.showModal = false;
    this.router.navigate(['/all-contractors']);
  }
  
}
