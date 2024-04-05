import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HotelNotFoundComponent } from './hotel-not-found.component';

describe('HotelNotFoundComponent', () => {
  let component: HotelNotFoundComponent;
  let fixture: ComponentFixture<HotelNotFoundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HotelNotFoundComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HotelNotFoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
