angular.module('appModule')
    .controller('UserRolesController', ['$scope', '$controller', '$routeParams', 'AccountService',
        function ($scope, $controller, $routeParams, accountService) {
            $controller('BaseSecuredController', {
                $scope: $scope
            });

            $scope.rolesOwner = $routeParams.userName;

            $scope.init = function () {
                $scope.authenticate();

                fillForm();
            }

            function fillForm() {
                $scope.selectedNotUserRole = -1;

                fillNotUserRoles();
                fillUserRoles();
            }

            function fillNotUserRoles() {
                accountService.getNotUserRoles(
                    $routeParams.userName,
                    onSuccessfulGettingNotUserRoles,
                    onFailedGettingNotUserRoles);
            }

            function onSuccessfulGettingNotUserRoles(data) {
                if (data) {
                    $scope.notUserRoles = data;

                    for (var i = 0; i < data.length; i++) {
                        $scope.notUserRoles[i].value = i;
                    }

                    if (data.length > 0) {
                        $scope.selectedNotUserRole = data[0].value
                    }
                }
            }

            $scope.onNotUserRoleChanged = function (index) {
                var selectedOption = $scope.notUserRoles[index];
                $scope.selectedNotUserRole = selectedOption.value;
            }

            function onFailedGettingNotUserRoles(data, code) {
                $scope.commonFailureCallback(data, code);
            }

            function fillUserRoles() {
                accountService.getUserRoles(
                    $routeParams.userName,
                    onSuccessfulGettingUserRoles,
                    onFailedGettingUserRoles);
            }

            function onSuccessfulGettingUserRoles(data) {
                $scope.roles = data;
            }

            function onFailedGettingUserRoles(data, code) {
                $scope.commonFailureCallback(data, code);
            }

            $scope.addRole = function () {
                var roleName = $scope.notUserRoles[$scope.selectedNotUserRole].roleName;
                accountService.addUserRole($routeParams.userName, roleName, onSuccessfulAddingRole, onFailedAddingRole);
            }

            function onSuccessfulAddingRole() {
                $scope.toastSuccess("Role has been successfully added.");
                fillForm();
            }

            function onFailedAddingRole(data, code) {
                $scope.commonFailureCallback(data, code);
            }

            $scope.deleteRole = function (roleName) {
                accountService.deleteUserRole($routeParams.userName, roleName, onSuccessfulDeletingRole, onFailedDeletingRole);
            }

            function onSuccessfulDeletingRole() {
                $scope.toastSuccess("Role has been successfully deleted.");
                fillForm();
            }

            function onFailedDeletingRole(data, code) {
                $scope.commonFailureCallback(data, code);
            }
        }
    ]);