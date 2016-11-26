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
                $scope.selectedNonUserRole = -1;

                fillNonUserRoles();
                fillUserRoles();
            }

            $scope.onNonUserRoleChanged = function (index) {
                var selectedOption = $scope.notUserRoles[index];
                $scope.selectedNonUserRole = selectedOption.value;
            }

            function fillNonUserRoles() {
                accountService.getNonUserRoles($routeParams.userName)
                    .then(
                        onSuccessfulGettingNonUserRoles,
                        $scope.onFailure);
            }

            function onSuccessfulGettingNonUserRoles(response) {
                var data = response.data;
                if (data) {
                    $scope.notUserRoles = data;

                    for (var i = 0; i < data.length; i++) {
                        $scope.notUserRoles[i].value = i;
                    }

                    if (data.length > 0) {
                        $scope.selectedNonUserRole = data[0].value
                    }
                }
            }

            function fillUserRoles() {
                accountService.getUserRoles($routeParams.userName)
                    .then(
                        onSuccessfulGettingUserRoles,
                        $scope.onFailure);
            }

            function onSuccessfulGettingUserRoles(response) {
                $scope.roles = response.data;
            }

            $scope.addRole = function () {
                var roleName = $scope.notUserRoles[$scope.selectedNonUserRole].roleName;
                accountService.addUserRole($routeParams.userName, roleName)
                    .then(
                        onSuccessfulAddingRole,
                        $scope.onFailure);
            }

            function onSuccessfulAddingRole() {
                $scope.toastSuccess("Role has been successfully added.");
                fillForm();
            }

            $scope.deleteRole = function (roleName) {
                accountService.deleteUserRole($routeParams.userName, roleName)
                    .then(
                        onSuccessfulDeletingRole,
                        $scope.onFailure);
            }

            function onSuccessfulDeletingRole() {
                $scope.toastSuccess("Role has been successfully deleted.");
                fillForm();
            }
        }
    ]);