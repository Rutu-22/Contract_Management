import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule, CommonModule, HttpClientModule,],
  templateUrl: `modal.component.html`,
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent {
  @Input() message: string = '';
  @Input() showConfirmButtons: boolean = false;

  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onConfirm(): void {
    this.confirm.emit();
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
