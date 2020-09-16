//import { Injectable } from '@angular/core';
//import { DataService } from '../shared/services/data.service';
//import { ConfigurationService } from '../shared/services/configuration.service';
//import { ICatalog } from './catalog.model';
//import { Observable } from "rxjs";
//import { tap } from 'rxjs/operators';
//@Injectable()
//export class CatalogService {
//  private catalogUrl: string = '';
//  constructor(private service: DataService, private configurationService: ConfigurationService) {
//    this.configurationService.settingsLoaded$.subscribe(x => {
//      this.catalogUrl = this.configurationService.serverSettings.catalogUrl + '/api/catalog/items';
//    });
//  }
//  getCatalog(): Observable<ICatalog> {
//    let url = this.catalogUrl;
//    return this.service.get(url).pipe<ICatalog>(tap((response: any) => {
//      return response;
//    }));
//  }
//}
//# sourceMappingURL=catalog.service.js.map