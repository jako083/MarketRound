import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MarketRoundComponent } from './components/market-round/market-round.component';
import { HeaderComponent } from './components/layout/header/header.component';
import { from } from 'rxjs';
import { MarketItemComponent } from './components/market-item/market-item.component';

@NgModule({
  declarations: [
    AppComponent,
    MarketRoundComponent,
    HeaderComponent,
    MarketItemComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
