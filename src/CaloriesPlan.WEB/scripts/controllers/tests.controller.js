angular.module('appModule')
    .controller('TestsController', ['$scope', '$controller', 'AccountService',
        function ($scope, $controller, accountService) {
            $controller('BaseSecuredController', {
                $scope: $scope
            });

            $scope.init = function () {
                $scope.authenticate();

                fillForm();
            }

            function fillForm() {
                accountService.getUsers()
                    .then(
                        onSuccessfulGettingUsers,
                        $scope.onFailure);
            }

            function onSuccessfulGettingUsers(response) {
                $scope.users = response.data;
            }
        }
    ]);