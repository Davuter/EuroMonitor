import { Component, OnInit } from '@angular/core';
import { ICatalog } from './catalog.model';
import { ConfigurationService } from '../shared/services/configuration.service';
import { CatalogService } from './catalog.service';
import { Observable, Subscription } from "rxjs";
import { SecurityService } from '../shared/services/security.service';
import { catchError } from 'rxjs/operators';


@Component({
  selector: 'esh-catalog .esh-catalog',
  styleUrls: ['./catalog.component.scss'],
  templateUrl: './catalog.component.html'
})
export class CatalogComponent implements OnInit {
  

  catalog: ICatalog;
  authenticated: boolean = false;
  authSubscription: Subscription;
  errorReceived: boolean;
  buyerId: string = '';

  constructor(private service: CatalogService, private configurationService: ConfigurationService, private securityService: SecurityService
    //, private toastr: ToastrService
  ) {
  
    debugger;
    console.log(this.configurationService);
    // http.get<ICatalogItem[]>('https://localhost:44373' + '/api/catalog/items').subscribe(result => {
    //  this.catalogitems = result;
    //}, error => console.error(error));
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
    this.getCatalog();
  }

  getCatalog() {
    this.errorReceived = false;
    this.service.getCatalog(this.buyerId)
      .pipe(catchError((err) => this.handleError(err)))
      .subscribe(catalog => {   
        this.catalog = catalog;       
      });
  }

  public unSubscription(item) {
    if (this.authenticated) {

      this.service.unSubcription(item, this.buyerId)
        .pipe(catchError((err) => this.handleError(err)))
        .subscribe(catalog => {
          debugger;
          console.log(catalog);
          this.loadData();
          //this.toastr.success('UnSubscription successfull','Unsubriction Detail');
        });
    } else {
      //this.toastr.warning('You have to login');
      this.securityService.Authorize();
    }   
  }

  public addSubcription(item) {
    debugger;
    if (this.authenticated) {
     
      this.service.addSubscription(item, this.buyerId)
        .pipe(catchError((err) => this.handleError(err)))
        .subscribe(catalog => {
          debugger;
          console.log(catalog);
          this.loadData();
          //this.toastr.success('Subscription successfull', 'Subriction Detail');
        });
    } else {
      //this.toastr.warning('You have to login');
      this.securityService.Authorize();
    }   
  }


  private handleError(error: any) {
    this.errorReceived = true;
    return Observable.throw(error);
  }
}
