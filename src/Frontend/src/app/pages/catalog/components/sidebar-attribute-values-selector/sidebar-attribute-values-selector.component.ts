import {Component, Input} from '@angular/core';
import {AttributeAllValues} from '../../../../data/interfaces/attributeAllValues.interface';

@Component({
  selector: 'app-sidebar-attribute-values-selector',
  imports: [],
  templateUrl: './sidebar-attribute-values-selector.component.html',
  styleUrl: './sidebar-attribute-values-selector.component.css'
})
export class SidebarAttributeValuesSelectorComponent {
  @Input() attributeValues!: AttributeAllValues;
  selectedValue: number | null = null;

  toggleRadio(value: number) {
    if (this.selectedValue === value) {
      this.selectedValue = null;
    } else {
      this.selectedValue = value;
    }
  }
}
