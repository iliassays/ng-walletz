'use strict';
app.factory('accountsService', ['$http', 'authSettings', function ($http, authSettings) {

    var getAccounts = function () {
        delete $http.defaults.headers.common['X-Requested-With'];
        return $http.get(authSettings.apiServiceBaseUri + 'api/account').then(function (results) {
            return results;
        });
    };

    return {
        getAccounts: getAccounts
    };
}]);