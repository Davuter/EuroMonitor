import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HttpClientJsonpModule } from '@angular/common/http';


// Services
import { DataService } from './services/data.service';
import { SecurityService } from './services/security.service';
import { ConfigurationService } from './services/configuration.service';
import { StorageService } from './services/storage.service';



// Components:
//import { Pager } from './components/pager/pager';
//import { Header } from './components/header/header';
import { Identity } from './components/identity/identity';
//import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';


// Pipes:
import { UppercasePipe } from './pipes/uppercase.pipe';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
  
    // No need to export as these modules don't expose any components/directive etc'
    HttpClientModule,
    HttpClientJsonpModule
  ],
  declarations: [
    //Pager,
    //Header,
    Identity,
    //PageNotFoundComponent,
    UppercasePipe
  ],
  exports: [
    // Modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    //NgbModule,
    //// Providers, Components, directive, pipes
    //Pager,
    //Header,
    Identity,
    //PageNotFoundComponent,
    UppercasePipe
  ]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [
        // Providers
        DataService,        
        SecurityService,
        ConfigurationService,
        StorageService        
      ]
    };
  }
}
