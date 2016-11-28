angular.module('appModule')
    .controller('FriendsListController', ['$scope', '$controller', 'AccountService',
        function ($scope, $controller, accountService) {
            $controller('BaseSecuredController', {
                $scope: $scope
            });

            $scope.init = function () {
                $scope.authenticate();

                fillForm();
            }

            function fillForm() {
                var userName = $routeParams.userName;
                if (userName && userName !== undefined) {
                    accountService.getFriends(userName)
                        .then(
                            onSuccessfulGettingFriends,
                            $scope.onFailure);
                }
            }

            function onSuccessfulGettingFriends(response) {
                $scope.accounts = response.data;
            }

            $scope.deleteFriend = function (friendName) {
                accountService.deleteAccount(friendName)
                    .then(
                        onSuccessfulDeletingFriend,
                        $scope.onFailure);
            }

            function onSuccessfulDeletingFriend() {
                $scope.toastSuccess("Friend has been successfully deleted.");
                fillForm();
            }
        }
    ]);