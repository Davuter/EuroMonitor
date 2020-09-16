import { Injectable } from '@angular/core';
import { DataService } from '../shared/services/data.service';
import { ConfigurationService } from '../shared/services/configuration.service';
import { SecurityService } from '../shared/services/security.service';
import { Observable } from 'rxjs';
import { UserSubscriptions } from './subscriptions.model';
import { tap } from 'rxjs/operators';

@Injectable()
export class SubscriptionsService {
  private subscriptionUrl: string = '';
  constructor(private service: DataService, private configurationService: ConfigurationService, private securityService: SecurityService) {
    this.configurationService.settingsLoaded$.subscribe(x => {
      if (securityService.IsAuthorized) {
        this.subscriptionUrl = this.configurationService.serverSettings.apiGatewayUrl + '/api/gateway/UserSubscriptions'
      }
    });
  }

  getUserSubscritions(buyerId): Observable<UserSubscriptions>{
    var req = {
      BuyerId: buyerId 
    };
    if (this.subscriptionUrl == '') {
      this.subscriptionUrl =this.configurationService.serverSettings.apiGatewayUrl + '/api/gateway/UserSubscriptions';
    }
    return this.service.post(this.subscriptionUrl, req)
      .pipe<UserSubscriptions>(tap((response: any) => {
      return response;
    }));

  }
}
