import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common'
import { SharedModule } from '../shared/shared.module';
import { SubscriptionsComponent } from './subscriptions.component';
import { SubscriptionsService } from './subscriptions.service';

@NgModule({
  imports: [BrowserModule, SharedModule, CommonModule],
  declarations: [SubscriptionsComponent],
  providers: [SubscriptionsService]
})
export class SubscriptionsModule { }
