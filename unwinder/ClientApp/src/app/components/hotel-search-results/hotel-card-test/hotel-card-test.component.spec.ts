import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HotelCardTestComponent } from './hotel-card-test.component';

describe('HotelCardTestComponent', () => {
  let component: HotelCardTestComponent;
  let fixture: ComponentFixture<HotelCardTestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HotelCardTestComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HotelCardTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
