angular.module('appModule')
    .controller('AccountController', ['$scope', '$controller', '$routeParams', 'AccountService', 'Navigator',
        function ($scope, $controller, $routeParams, accountService, navigator) {
            $controller('BaseSecuredController', {
                $scope: $scope
            });

            $scope.accountModel = {
                userName: "",
                dailyCaloriesLimit: "",
                userRoles: []
            };

            $scope.init = function () {
                $scope.authenticate();

                var userName = $routeParams.userName;
                if (userName && userName !== undefined) {
                    accountService.getAccount(userName)
                        .then(
                            onSuccessfulGettingAccount,
                            $scope.onFailure);

                    accountService.getSubscribers(userName)
                        .then(
                            onSuccessfulGettingSubscribers,
                            $scope.onFailure);
                }
            }

            function onSuccessfulGettingAccount(response) {
                $scope.accountModel = response.data;
            }

            function onSuccessfulGettingSubscribers(response) {
                $scope.subscribers = response.data;
            }

            $scope.save = function () {
                $scope.clearInfoMessages();
                
                accountService.saveAccount($scope.accountModel)
                    .then(
                        onSuccessfulSave,
                        $scope.onFailure);
            }

            function onSuccessfulSave() {
                $scope.toastSuccess("Account has been successfully saved.");
                $scope.goBack();
            }

            $scope.editRoles = function () {
                var rolesOwner = $routeParams.userName;
                navigator.goToEditRoles(rolesOwner);
            }

            $scope.goBack = function () {
                if (accountService.isUserHasRoles(["Admin", "Manager"])) {
                    navigator.goToAccountsList();
                } else {
                    navigator.goToNutritionReport($scope.userName);
                }
            }
        }
    ]);
