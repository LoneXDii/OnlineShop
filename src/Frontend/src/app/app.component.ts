import {Component} from '@angular/core';
import {Category} from './data/interfaces/category.interface';
import {CatalogComponent} from './pages/catalog/catalog.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [
    CatalogComponent
  ],
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Frontend';

  category: Category;

  constructor() {
    this.category = {
      id: 1,
      name: 'Test',
      imageUrl: 'https://example.com/image.jpg'
    };
  }
}
