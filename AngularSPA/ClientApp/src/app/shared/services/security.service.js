"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/common/http");
var rxjs_1 = require("rxjs");
var SecurityService = /** @class */ (function () {
    function SecurityService(_http, _router, route, _configurationService, _storageService) {
        var _this = this;
        this._http = _http;
        this._router = _router;
        this.route = route;
        this._configurationService = _configurationService;
        this._storageService = _storageService;
        this.authenticationSource = new rxjs_1.Subject();
        this.authenticationChallenge$ = this.authenticationSource.asObservable();
        this.authorityUrl = '';
        this.getUserData = function () {
            if (_this.authorityUrl === '') {
                _this.authorityUrl = _this.storage.retrieve('IdentityUrl');
            }
            var options = _this.setHeaders();
            return _this._http.get(_this.authorityUrl + "/connect/userinfo", options)
                .pipe(function (info) { return info; });
        };
        this.headers = new http_1.HttpHeaders();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
        this.storage = _storageService;
        this._configurationService.settingsLoaded$.subscribe(function (x) {
            _this.authorityUrl = _this._configurationService.serverSettings.identityUrl;
            _this.storage.store('IdentityUrl', _this.authorityUrl);
        });
        if (this.storage.retrieve('IsAuthorized') !== '') {
            this.IsAuthorized = this.storage.retrieve('IsAuthorized');
            this.authenticationSource.next(true);
            this.UserData = this.storage.retrieve('userData');
        }
    }
    SecurityService.prototype.GetToken = function () {
        return this.storage.retrieve('authorizationData');
    };
    SecurityService.prototype.ResetAuthorizationData = function () {
        this.storage.store('authorizationData', '');
        this.storage.store('authorizationDataIdToken', '');
        this.IsAuthorized = false;
        this.storage.store('IsAuthorized', false);
    };
    SecurityService.prototype.SetAuthorizationData = function (token, id_token) {
        var _this = this;
        if (this.storage.retrieve('authorizationData') !== '') {
            this.storage.store('authorizationData', '');
        }
        this.storage.store('authorizationData', token);
        this.storage.store('authorizationDataIdToken', id_token);
        this.IsAuthorized = true;
        this.storage.store('IsAuthorized', true);
        this.getUserData()
            .subscribe(function (data) {
            _this.UserData = data;
            _this.storage.store('userData', data);
            // emit observable
            _this.authenticationSource.next(true);
            window.location.href = location.origin;
        }, function (error) { return _this.HandleError(error); }, function () {
            console.log(_this.UserData);
        });
    };
    SecurityService.prototype.Authorize = function () {
        this.ResetAuthorizationData();
        var authorizationUrl = this.authorityUrl + '/connect/authorize';
        var client_id = 'js';
        var redirect_uri = location.origin + '/';
        var response_type = 'id_token token';
        var scope = 'openid profile orders basket marketing locations webshoppingagg orders.signalrhub';
        var nonce = 'N' + Math.random() + '' + Date.now();
        var state = Date.now() + '' + Math.random();
        this.storage.store('authStateControl', state);
        this.storage.store('authNonce', nonce);
        var url = authorizationUrl + '?' +
            'response_type=' + encodeURI(response_type) + '&' +
            'client_id=' + encodeURI(client_id) + '&' +
            'redirect_uri=' + encodeURI(redirect_uri) + '&' +
            'scope=' + encodeURI(scope) + '&' +
            'nonce=' + encodeURI(nonce) + '&' +
            'state=' + encodeURI(state);
        window.location.href = url;
    };
    SecurityService.prototype.AuthorizedCallback = function () {
        this.ResetAuthorizationData();
        var hash = window.location.hash.substr(1);
        var result = hash.split('&').reduce(function (result, item) {
            var parts = item.split('=');
            result[parts[0]] = parts[1];
            return result;
        }, {});
        console.log(result);
        var token = '';
        var id_token = '';
        var authResponseIsValid = false;
        if (!result.error) {
            if (result.state !== this.storage.retrieve('authStateControl')) {
                console.log('AuthorizedCallback incorrect state');
            }
            else {
                token = result.access_token;
                id_token = result.id_token;
                var dataIdToken = this.getDataFromToken(id_token);
                // validate nonce
                if (dataIdToken.nonce !== this.storage.retrieve('authNonce')) {
                    console.log('AuthorizedCallback incorrect nonce');
                }
                else {
                    this.storage.store('authNonce', '');
                    this.storage.store('authStateControl', '');
                    authResponseIsValid = true;
                    console.log('AuthorizedCallback state and nonce validated, returning access token');
                }
            }
        }
        if (authResponseIsValid) {
            this.SetAuthorizationData(token, id_token);
        }
    };
    SecurityService.prototype.Logoff = function () {
        var authorizationUrl = this.authorityUrl + '/connect/endsession';
        var id_token_hint = this.storage.retrieve('authorizationDataIdToken');
        var post_logout_redirect_uri = location.origin + '/';
        var url = authorizationUrl + '?' +
            'id_token_hint=' + encodeURI(id_token_hint) + '&' +
            'post_logout_redirect_uri=' + encodeURI(post_logout_redirect_uri);
        this.ResetAuthorizationData();
        // emit observable
        this.authenticationSource.next(false);
        window.location.href = url;
    };
    SecurityService.prototype.HandleError = function (error) {
        console.log(error);
        if (error.status == 403) {
            this._router.navigate(['/Forbidden']);
        }
        else if (error.status == 401) {
            // this.ResetAuthorizationData();
            this._router.navigate(['/Unauthorized']);
        }
    };
    SecurityService.prototype.urlBase64Decode = function (str) {
        var output = str.replace('-', '+').replace('_', '/');
        switch (output.length % 4) {
            case 0:
                break;
            case 2:
                output += '==';
                break;
            case 3:
                output += '=';
                break;
            default:
                throw 'Illegal base64url string!';
        }
        return window.atob(output);
    };
    SecurityService.prototype.getDataFromToken = function (token) {
        var data = {};
        if (typeof token !== 'undefined') {
            var encoded = token.split('.')[1];
            data = JSON.parse(this.urlBase64Decode(encoded));
        }
        return data;
    };
    SecurityService.prototype.setHeaders = function () {
        var httpOptions = {
            headers: new http_1.HttpHeaders()
        };
        httpOptions.headers = httpOptions.headers.set('Content-Type', 'application/json');
        httpOptions.headers = httpOptions.headers.set('Accept', 'application/json');
        var token = this.GetToken();
        if (token !== '') {
            httpOptions.headers = httpOptions.headers.set('Authorization', "Bearer " + token);
        }
        return httpOptions;
    };
    return SecurityService;
}());
exports.SecurityService = SecurityService;
//# sourceMappingURL=security.service.js.map