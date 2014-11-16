var app = angular.module(
    'myWalletz',
    [
        'ngRoute',
        'LocalStorageModule',
        'angular-loading-bar'
    ]);

app.constant('authSettings', {
    apiServiceBaseUri: 'http://localhost/appauthapi/',
    clientId: 'myWalletz'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);






