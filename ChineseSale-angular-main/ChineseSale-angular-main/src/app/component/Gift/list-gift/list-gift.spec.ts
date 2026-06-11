import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListGift } from './list-gift';

describe('ListGift', () => {
  let component: ListGift;
  let fixture: ComponentFixture<ListGift>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListGift]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListGift);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
