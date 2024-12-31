import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCardsLayotComponent } from './product-cards-layout.component';

describe('ProductCardsLayotComponent', () => {
  let component: ProductCardsLayotComponent;
  let fixture: ComponentFixture<ProductCardsLayotComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductCardsLayotComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductCardsLayotComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
