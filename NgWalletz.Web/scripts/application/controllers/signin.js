'use strict';
app.controller('SignIn', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.loginData = {
        userName: "",
        password: "",
        useRefreshTokens: false
    };

    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {

            $location.path('/accounts');

        },
         function (err) {
             $scope.message = err.error_description;
         });
    };

}]);