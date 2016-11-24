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

                accountService.signIn($scope.signInModel)
                    .then(
                        onSuccessfulSignIn,
                        $scope.onFailure);
            }

            function onSuccessfulSignIn() {
                navigator.goToNutritionReport($scope.signInModel.userName);
            }
        }
    ]);
