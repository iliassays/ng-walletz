app.config(function ($routeProvider) {

    $routeProvider.
        when('/', {
            controller: "Page",
            templateUrl: "/views/home.html"
        }).
        when('/sign-in', {
            controller: "SignIn",
            templateUrl: "/views/session-create.html"
        }).
        when('/sign-up', {
            controller: "SignUp",
            templateUrl: "/views/user-create.html"
        }).
        when('/accounts', {
            controller: "Accounts",
            templateUrl: "/views/accounts.html",
            secure: true
        }).
        when('/refresh', {
            controller: "Refresh",
            templateUrl: "/views/refresh.html",
            secure: true
        }).
        when('/tokens', {
            controller: "Tokens",
            templateUrl: "/views/tokens.html",
            secure: true
        }).
        when('/404', {
            templateUrl: 'not-found.html',
            controller: 'Page'
        }).
        otherwise({
            redirectTo: '/404'
        });
});