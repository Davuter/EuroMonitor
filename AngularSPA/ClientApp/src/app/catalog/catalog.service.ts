import { Injectable } from '@angular/core';
import { DataService } from '../shared/services/data.service';
import { ConfigurationService } from '../shared/services/configuration.service';
import { ICatalog } from './catalog.model';
import { SubscriptionCancelResponse } from './catalog.cancelModel';
import { Observable } from "rxjs";
import { tap } from 'rxjs/operators';
import { SecurityService } from '../shared/services/security.service';

@Injectable()
export class CatalogService {
  private catalogUrl: string = '';

  constructor(private service: DataService, private configurationService: ConfigurationService, private securityService: SecurityService) {
    this.configurationService.settingsLoaded$.subscribe(x => {
      if (securityService.IsAuthorized) {
        this.catalogUrl = this.configurationService.serverSettings.apiGatewayUrl + '/api/gateway/CatalogItemsWithSubcripted'
      } else {
        this.catalogUrl = this.configurationService.serverSettings.apiGatewayUrl + '/api/gateway/CatalogItems'
      }
     ;
    });
  }

  getCatalog(buyerId): Observable<ICatalog> {
    debugger;
    let url = this.catalogUrl;
    if (buyerId != '') {

        var req = {
          BuyerId: buyerId
          };
        return this.service.post(url, req).pipe<ICatalog>(tap((response: any) => {
          return response;
        }));
      
     
    } else {
      return this.service.get(url).pipe<ICatalog>(tap((response: any) => {
        return response;
      }));
    }
   
  }

  addSubscription(catalogItem, buyerId): Observable<number> {
    var url = this.configurationService.serverSettings.apiGatewayUrl + '/api/gateway/AddUserSubscription';
    var req = {
      BuyerId: buyerId,
      ProductId: catalogItem.id,
      ProductName: catalogItem.name
    };
    return this.service.post(url, req).pipe<number>(tap((response: any) => {
      return response;
    }));
  }

  unSubcription(catalogItem, buyerId): Observable<SubscriptionCancelResponse> {
    var url = this.configurationService.serverSettings.apiGatewayUrl + '/api/gateway/CancelUserSubscription';
    var req = {
      BuyerId: buyerId,
      ProductId: catalogItem.id,
      Id: catalogItem.subscriptedId
    };
    return this.service.post(url, req).pipe<SubscriptionCancelResponse>(tap((response: any) => {
      return response;
    }));
  }
}
