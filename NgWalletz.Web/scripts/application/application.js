var app = angular.module(
    'ngWalletz',
    [
        'ngRoute',
        'LocalStorageModule',
        'angular-loading-bar'
    ]);

app.constant('authSettings', {
    apiServiceBaseUri: 'http://localhost:26265/',
    clientId: 'NgWalletz'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
    //$httpProvider.defaults.useXDomain = true;
    //delete $httpProvider.defaults.headers.common['X-Requested-With'];
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);






