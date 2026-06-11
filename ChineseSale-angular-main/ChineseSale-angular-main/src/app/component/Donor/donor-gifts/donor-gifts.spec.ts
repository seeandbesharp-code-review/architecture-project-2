import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonorGifts } from './donor-gifts';

describe('DonorGifts', () => {
  let component: DonorGifts;
  let fixture: ComponentFixture<DonorGifts>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonorGifts]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DonorGifts);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
