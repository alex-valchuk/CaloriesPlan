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
                accountService.getAccounts()
                    .then(
                        onSuccessfulGettingAccounts,
                        $scope.onFailure);
            }

            function onSuccessfulGettingAccounts(response) {
                $scope.accounts = response.data;
            }

            $scope.deleteAccount = function (userName) {
                accountService.deleteAccount(userName)
                    .then(
                        onSuccessfulDeletingAccount,
                        $scope.onFailure);
            }

            function onSuccessfulDeletingAccount() {
                $scope.toastSuccess("Account has been successfully deleted.");
                fillForm();
            }
        }
    ]);