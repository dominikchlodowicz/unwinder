import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HotelSearchResultsComponent } from './hotel-search-results.component';
import { UnwinderSessionService } from '../../../services/unwinder-search-state/unwinder-session.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HotelResponseData } from '../../../interfaces/hotel-data-exchange/hotel-response-data';
import { RouterTestingModule } from '@angular/router/testing';
import { HotelCardComponent } from '../hotel-card/hotel-card.component';

describe('HotelSearchResultsComponent', () => {
  let component: HotelSearchResultsComponent;
  let fixture: ComponentFixture<HotelSearchResultsComponent>;
  let mockUnwinderSessionService = {
    getData: jest.fn(),
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        HotelCardComponent,
      ],
      providers: [
        {
          provide: UnwinderSessionService,
          useValue: mockUnwinderSessionService,
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(HotelSearchResultsComponent);
    component = fixture.componentInstance;

    mockUnwinderSessionService.getData.mockReturnValue({} as HotelResponseData);

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
