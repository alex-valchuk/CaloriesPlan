angular.module('appModule')
    .controller('LoginController', ['$scope', '$controller', 'AccountService', 'Navigator',
        function ($scope, $controller, accountService, navigator) {
            $controller('BaseController', {
                $scope: $scope
            });

            $scope.loginModel = {
                userName: "",
                password: ""
            };

            $scope.init = function () {
                
            }

            $scope.signin = function () {
                $scope.clearInfoMessages();

                accountService.login(
                    $scope.loginModel.userName,
                    $scope.loginModel.password,
                    onSuccessfullLogin,
                    onFailedLogin);
            }

            function onSuccessfullLogin() {
                navigator.goToNutritionReport($scope.loginModel.userName);
            }

            function onFailedLogin(data, code) {
                $scope.commonFailureCallback(data, code);
            }
        }
    ]);
