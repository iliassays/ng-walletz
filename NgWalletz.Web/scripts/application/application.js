var app = angular.module(
    'ngWalletz',
    [
        'ngRoute',
        'LocalStorageModule',
        'angular-loading-bar'
    ]);

app.constant('authSettings', {
    apiServiceBaseUri: 'http://localhost/ngwalletzapi/',
    clientId: 'NgWalletz'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);






