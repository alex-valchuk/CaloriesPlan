angular.module('appModule')
    .controller('RegisterController', ['$scope', '$controller', 'AccountService', 'Navigator',
        function ($scope, $controller, accountService, navigator) {
            $controller('BaseController', {
                $scope: $scope
            });

            $scope.signupText = "Sign Up";
            $scope.cancelText = "Sign In";

            $scope.registerModel = {
                userName: "",
                password: "",
                confirmPassword: ""
            };

            $scope.init = function () {
                if (navigator.isAccountView() === true) {
                    $scope.signupText = "Add New Account";
                    $scope.cancelText = "Cancel";
                }
            }

            $scope.signup = function () {
                $scope.clearInfoMessages();

                if (isValidPasswordConfirmation() == false) {
                    $scope.addError("The password and confirmation password do not match.");
                    return;
                }
                
                accountService.register(
                    $scope.registerModel.userName,
                    $scope.registerModel.password,
                    $scope.registerModel.confirmPassword,
                    onSuccessfulRegistration,
                    onFailedRegistration);
            };

            $scope.goBack = function () {
                if (navigator.isAccountView() === true) {
                    navigator.goToAccountsList();
                }
                else {
                    navigator.goToSignIn();
                }
            }

            function isValidPasswordConfirmation () {
                return
                    $scope.registerModel.password ===
                    $scope.registerModel.confirmPassword;
            }

            function onSuccessfulRegistration () {
                $scope.toastSuccess("User has been successfully registered.");
                $scope.goBack();
            }

            function onFailedRegistration (data, code) {
                $scope.commonFailureCallback(data, code);
            }
        }
    ]);