import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Baglist } from './bag-list';

describe('BagList', () => {
  let component: Baglist;
  let fixture: ComponentFixture<Baglist>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Baglist]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Baglist);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
