import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllWinners } from './all-winners';

describe('AllWinners', () => {
  let component: AllWinners;
  let fixture: ComponentFixture<AllWinners>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllWinners]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllWinners);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
