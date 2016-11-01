angular.module('appModule')
    .controller('BaseSecuredController', ['$scope', '$controller', '$routeParams', 'AccountService', 'Navigator',
        function ($scope, $controller, $routeParams, accountService, navigator) {
            $controller('BaseController', {
                $scope: $scope
            });

            $scope.requestedUser = $routeParams.userName;

            $scope.authenticate = function () {
                $scope.isAuthenticated = accountService.isAuthenticated();

                if ($scope.isAuthenticated === true) {

                    $scope.userName = accountService.getUserName();
                } else {

                    navigator.goToLogin();
                }
            }

            $scope.isAdmin = function () {
                return accountService.isUserHasRoles(["Admin"]);
            }

            $scope.isAdminOrManager = function () {
                return accountService.isUserHasRoles(["Admin", "Manager"]);
            }
        }
    ]);
