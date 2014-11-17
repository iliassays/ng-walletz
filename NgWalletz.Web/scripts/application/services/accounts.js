'use strict';
app.factory('accountsService', ['$http', 'authSettings', function ($http, authSettings) {

    var getAccounts = function () {

        return $http.get(authSettings.apiServiceBaseUri + 'api/account').then(function (results) {
            return results;
        });
    };

    return {
        getAccounts: getAccounts
    };
}]);