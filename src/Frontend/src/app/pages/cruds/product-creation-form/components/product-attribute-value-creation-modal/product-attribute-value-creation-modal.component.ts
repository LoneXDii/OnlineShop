import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {Category} from '../../../../../data/interfaces/catalog/category.interface';
import {CategoriesService} from '../../../../../data/services/categories.service';

@Component({
  selector: 'app-product-attribute-value-creation-modal',
  imports: [
    FormsModule
  ],
  templateUrl: './product-attribute-value-creation-modal.component.html',
  styleUrl: './product-attribute-value-creation-modal.component.css'
})
export class ProductAttributeValueCreationModalComponent {
  @Input() attribute!: Category;
  @Output() closeModal = new EventEmitter<void>();
  categoriesService = inject(CategoriesService);
  attributeValue: string = '';

  onClose() {
    this.closeModal.emit();
  }

  onSubmit() {
    this.categoriesService.createChildCategory({
      parentId: this.attribute.id,
      name: this.attributeValue,
    })
    .subscribe(() => this.closeModal.emit());
  }
}
