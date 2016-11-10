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
                    accountService.getAccount(
                        userName,
                        onSuccessfulGettingAccount,
                        onFailedGettingAccount);
                }
            }

            function onSuccessfulGettingAccount(data) {
                $scope.accountModel = data;
            }

            function onFailedGettingAccount(data, code) {
                $scope.commonFailureCallback(data, code);
            }

            $scope.save = function () {
                $scope.clearInfoMessages();
                
                accountService.saveAccount(
                    $scope.accountModel.userName,
                    $scope.accountModel.dailyCaloriesLimit,
                    onSuccessfullSave,
                    onFailedSave);
            }

            function onSuccessfullSave() {
                $scope.toastSuccess("Account has been successfully saved.");
                $scope.goBack();
            }

            function onFailedSave(data, code) {
                $scope.commonFailureCallback(data, code);
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
