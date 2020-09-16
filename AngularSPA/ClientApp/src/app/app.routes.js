"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var router_1 = require("@angular/router");
var catalog_component_1 = require("./catalog/catalog.component");
var subscriptions_component_1 = require("./subscriptions/subscriptions.component");
exports.routes = [
    { path: '', redirectTo: 'catalog', pathMatch: 'full' },
    { path: 'catalog', component: catalog_component_1.CatalogComponent },
    { path: 'subscriptions', component: subscriptions_component_1.SubscriptionsComponent }
];
exports.routing = router_1.RouterModule.forRoot(exports.routes);
//# sourceMappingURL=app.routes.js.map