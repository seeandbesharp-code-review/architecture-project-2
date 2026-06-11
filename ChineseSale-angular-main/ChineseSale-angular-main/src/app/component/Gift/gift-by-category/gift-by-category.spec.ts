import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftByCategory } from './gift-by-category';

describe('GiftByCategory', () => {
  let component: GiftByCategory;
  let fixture: ComponentFixture<GiftByCategory>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiftByCategory]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GiftByCategory);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
