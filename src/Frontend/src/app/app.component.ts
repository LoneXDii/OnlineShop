import {Component} from '@angular/core';
import {Category} from './data/interfaces/catalog/category.interface';
import {NavigationComponent} from './pages/common/navigation/navigation.component';
import {RouterOutlet} from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [
    NavigationComponent,
    RouterOutlet
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
