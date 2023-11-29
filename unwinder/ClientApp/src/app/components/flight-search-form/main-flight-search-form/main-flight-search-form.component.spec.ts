import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainFlightSearchFormComponent } from './main-flight-search-form.component';

describe('MainFlightSearchFormComponent', () => {
  let component: MainFlightSearchFormComponent;
  let fixture: ComponentFixture<MainFlightSearchFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainFlightSearchFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(MainFlightSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
