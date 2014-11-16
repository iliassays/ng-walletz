'use strict';
app.factory('tokensManagerService', ['$http', 'authSettings', function ($http, authSettings) {

    var serviceBase = authSettings.apiServiceBaseUri;
    
    var tokenManagerServiceFactory = {};

    var getRefreshTokens = function () {

        return $http.get(serviceBase + 'api/refreshtokens').then(function (results) {
            return results;
        });
    };

    var deleteRefreshTokens = function (tokenid) {

        return $http.delete(serviceBase + 'api/refreshtokens/?tokenid=' + tokenid).then(function (results) {
            return results;
        });
    };

    return {
        deleteRefreshTokens: deleteRefreshTokens,
        getRefreshTokens: getRefreshTokens
    };

}]);