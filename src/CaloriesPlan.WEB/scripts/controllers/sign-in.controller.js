angular.module('appModule')
    .controller('SignInController', ['$scope', '$controller', 'AccountService', 'Navigator',
        function ($scope, $controller, accountService, navigator) {
            $controller('BaseController', {
                $scope: $scope
            });

            $scope.signInModel = {
                userName: "",
                password: ""
            };

            $scope.init = function () {
                
            }

            $scope.signIn = function () {
                $scope.clearInfoMessages();

                accountService.signIn(
                    $scope.signInModel.userName,
                    $scope.signInModel.password)
                    .then(
                        onSuccessfullSignIn,
                        $scope.onFailure);
            }

            function onSuccessfullSignIn() {
                navigator.goToNutritionReport($scope.signInModel.userName);
            }
        }
    ]);
