import { Component, OnInit } from '@angular/core';
import { MarketService} from '../../services/market.service'
import { MarketRound } from '../../models/MarketRound';

@Component({
  selector: 'app-market-round',
  templateUrl: './market-round.component.html',
  styleUrls: ['./market-round.component.css']
})
export class MarketRoundComponent implements OnInit {
  MarketRound:MarketRound[];

  constructor(private marketService:MarketService) { }

  ngOnInit(): void {
    this.marketService.getMarket().subscribe(MarketRound => {
      this.MarketRound = MarketRound;
    });
  }

}
