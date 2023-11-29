import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WhereToFlightSearchFormComponent } from './where-to-flight-search-form.component';

describe('WhereToFlightSearchFormComponent', () => {
  let component: WhereToFlightSearchFormComponent;
  let fixture: ComponentFixture<WhereToFlightSearchFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WhereToFlightSearchFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(WhereToFlightSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
