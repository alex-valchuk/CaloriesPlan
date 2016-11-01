angular.module('appModule')
    .controller('AccountsListController', ['$scope', '$controller', 'AccountService',
        function ($scope, $controller, accountService) {
            $controller('BaseSecuredController', {
                $scope: $scope
            });

            $scope.init = function () {
                $scope.authenticate();

                fillForm();
            }

            function fillForm() {
                accountService.getAccounts(
                    onSuccessfulGettingAccounts,
                    onFailedGettingAccounts);
            }

            function onSuccessfulGettingAccounts(data) {
                $scope.accounts = data;
            }

            function onFailedGettingAccounts(data, code) {
                $scope.commonFailureCallback(data, code);
            }

            $scope.deleteAccount = function (userName) {
                accountService.deleteAccount(userName, onSuccessfulDeletingAccount, onFailedDeletingAccount);
            }

            function onSuccessfulDeletingAccount() {
                $scope.toastSuccess("Account has been successfully deleted.");
                fillForm();
            }

            function onFailedDeletingAccount(data, code) {
                $scope.commonFailureCallback(data, code);
            }
        }
    ]);