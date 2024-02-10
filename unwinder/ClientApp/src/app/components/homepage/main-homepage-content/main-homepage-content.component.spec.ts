import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainHomepageContentComponent } from './main-homepage-content.component';

describe('MainHomepageContentComponent', () => {
  let component: MainHomepageContentComponent;
  let fixture: ComponentFixture<MainHomepageContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainHomepageContentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MainHomepageContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
