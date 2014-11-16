﻿'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', 'authSettings', function ($http, $q, localStorageService, authSettings) {

    var serviceBase = authSettings.apiServiceBaseUri;

    var authentication = {
        isAuth: false,
        userName: "",
        useRefreshTokens: false
    };

    var saveRegistration = function (registration) {

        logOut();

        return $http.post(serviceBase + 'api/user/register', registration).then(function (response) {
            return response;
        });

    };

    var login = function (loginData) {
       
        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

        if (loginData.useRefreshTokens) {
            data = data + "&client_id=" + authSettings.clientId;
        }

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

            if (loginData.useRefreshTokens) {
                localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
            }
            else {
                localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false });
            }

            authentication.isAuth = true;
            authentication.userName = loginData.userName;
            authentication.useRefreshTokens = loginData.useRefreshTokens;

            deferred.resolve(response);

        }).error(function (err, status) {
            logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var logOut = function () {

        localStorageService.remove('authorizationData');

        authentication.isAuth = false;
        authentication.userName = "";
        authentication.useRefreshTokens = false;

    };

    var fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');

        if (authData) {
            authentication.isAuth = true;
            authentication.userName = authData.userName;
            authentication.useRefreshTokens = authData.useRefreshTokens;
        }

    };

    var refreshToken = function ()
    {
        var deferred = $q.defer();
       
        var authData = localStorageService.get('authorizationData');

        if (authData) {

            if (authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + authSettings.clientId;

                localStorageService.remove('authorizationData');

                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                    localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });

                    deferred.resolve(response);

                }).error(function (err, status) {
                    logOut();
                    deferred.reject(err);
                });
            }
        }

        return deferred.promise;
    };

    return {
        saveRegistration: saveRegistration,
        login: login,
        logOut: logOut,
        fillAuthData: fillAuthData,
        authentication: authentication,
        refreshToken: refreshToken
    };
}]);