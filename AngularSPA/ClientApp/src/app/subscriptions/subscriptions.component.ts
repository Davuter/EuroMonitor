import { Component, OnInit } from '@angular/core';
import { ConfigurationService } from '../shared/services/configuration.service';
import { Observable, Subscription } from "rxjs";
import { SecurityService } from '../shared/services/security.service';
import { catchError } from 'rxjs/operators';
import { SubscriptionsService } from './subscriptions.service';
import { UserSubscriptions } from './subscriptions.model';


@Component({
  selector: 'esh-subscriptions .esh-subscriptions',
  styleUrls: ['./subscriptions.component.scss'],
  templateUrl: './subscriptions.component.html'
})

export class SubscriptionsComponent implements OnInit {
  items: UserSubscriptions;
  authenticated: boolean = false;
  authSubscription: Subscription;
  errorReceived: boolean;
  buyerId: string = '';


  constructor(private service: SubscriptionsService, private configurationService: ConfigurationService, private securityService: SecurityService) {

    debugger;
    console.log(this.configurationService);
    
    this.authenticated = securityService.IsAuthorized;
    if (securityService.IsAuthorized) {
      if (securityService.UserData) {
        this.buyerId = securityService.UserData.sub;
      }
    }

  }
  ngOnInit() {

    // Configuration Settings:
    if (this.configurationService.isReady)
      this.loadData();
    else
      this.configurationService.settingsLoaded$.subscribe(x => {
        this.loadData();
      });

    // Subscribe to login and logout observable
    this.authSubscription = this.securityService.authenticationChallenge$.subscribe(res => {
      this.authenticated = res;
    });
  }

  loadData() {
    this.getSubscriptions();
  }

  getSubscriptions() {
    this.errorReceived = false;
    this.service.getUserSubscritions(this.buyerId)
      .pipe(catchError((err) => this.handleError(err)))
      .subscribe(subscriptions => {
        console.log(JSON.stringify(subscriptions));
        this.items = subscriptions;
      });
  }


  private handleError(error: any) {
    this.errorReceived = true;
    return Observable.throw(error);
  }
}
