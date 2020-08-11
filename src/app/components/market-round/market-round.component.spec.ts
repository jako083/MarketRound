import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MarketRoundComponent } from './market-round.component';

describe('MarketRoundComponent', () => {
  let component: MarketRoundComponent;
  let fixture: ComponentFixture<MarketRoundComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MarketRoundComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MarketRoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
