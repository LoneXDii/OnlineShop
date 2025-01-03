import {Component, EventEmitter, Input, Output} from '@angular/core';
import {AttributeAllValues} from '../../../../data/interfaces/catalog/attributeAllValues.interface';

@Component({
  selector: 'app-sidebar-attribute-values-selector',
  imports: [],
  templateUrl: './sidebar-attribute-values-selector.component.html',
  styleUrl: './sidebar-attribute-values-selector.component.css'
})
export class SidebarAttributeValuesSelectorComponent {
  @Input() attributeValues!: AttributeAllValues;
  @Output() valueSelected = new EventEmitter<number>();
  selectedValue: number | null = null;

  toggleRadio(value: number) {
    if (this.selectedValue === value) {
      this.valueSelected.emit(undefined);
      this.selectedValue = null;
    } else {
      this.valueSelected.emit(value);
      this.selectedValue = value;
    }
  }

  onSelect(value: number) {
    this.valueSelected.emit(value);
  }
}
