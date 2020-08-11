import { Component, OnInit, Input } from '@angular/core';
import { MarketRound } from 'src/app/models/MarketRound';

@Component({
  selector: 'app-market-item',
  templateUrl: './market-item.component.html',
  styleUrls: ['./market-item.component.css']
})
export class MarketItemComponent implements OnInit {
  @Input() market: MarketRound;

  constructor() { }

  ngOnInit(): void {
  }

}
