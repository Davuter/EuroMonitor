import { Component,OnInit } from '@angular/core';
import { SecurityService } from './shared/services/security.service';
import { ConfigurationService } from './shared/services/configuration.service';
import { Title } from '@angular/platform-browser';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  Authenticated: boolean = false;
  subscription: Subscription;

  constructor(private titleService: Title,
    private securityService: SecurityService,
    private configurationService: ConfigurationService,

  ) {
    // TODO: Set Taster Root (Overlay) container
    //this.toastr.setRootViewContainerRef(vcr);
    this.Authenticated = this.securityService.IsAuthorized;
  }

  ngOnInit() {
    console.log('app on init');
    this.subscription = this.securityService.authenticationChallenge$.subscribe(res => this.Authenticated = res);

    //Get configuration from server environment variables:
    console.log('configuration');
    this.configurationService.load();
  }

  public setTitle(newTitle: string) {
    this.titleService.setTitle('eShopOnContainers');
  }
}
