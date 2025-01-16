import {Component, EventEmitter, Input, Output} from '@angular/core';
import {FormGroup, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-category-form',
  imports: [
    FormsModule,
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './category-form.component.html',
  styleUrl: './category-form.component.css'
})
export class CategoryFormComponent {
  @Input() form!: FormGroup;
  @Output() submitForm = new EventEmitter<FormData>();

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.form.patchValue({
      image: file
    });
  }

  onSubmit(){
    if (this.form.valid) {
      const formData = new FormData();

      formData.append('name', this.form.get('name')?.value || '');
      formData.append('image', this.form.get('image')?.value || '');

      this.submitForm.emit(formData);
    }
  }
}
