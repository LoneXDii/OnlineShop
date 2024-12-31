import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidebarAttributeValuesSelectorComponent } from './sidebar-attribute-values-selector.component';

describe('SidebarAttributeValuesSelectorComponent', () => {
  let component: SidebarAttributeValuesSelectorComponent;
  let fixture: ComponentFixture<SidebarAttributeValuesSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SidebarAttributeValuesSelectorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SidebarAttributeValuesSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
