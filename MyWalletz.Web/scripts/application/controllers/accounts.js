'use strict';
app.controller('Accounts', ['$scope', 'accountsService', function ($scope, accountsService) {

    $scope.accounts = [];

    accountsService.getAccounts().then(function (results) {

        $scope.accounts = results.data;

    }, function (error) {
        
    });

}]);