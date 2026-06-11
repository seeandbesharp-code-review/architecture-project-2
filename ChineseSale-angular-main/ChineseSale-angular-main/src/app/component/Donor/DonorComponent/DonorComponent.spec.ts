import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonorComponent } from './DonorComponent';
import { ButtonModule } from 'primeng/button';

describe('DonorComponent', () => {
  let component: DonorComponent;
  let fixture: ComponentFixture<DonorComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DonorComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
