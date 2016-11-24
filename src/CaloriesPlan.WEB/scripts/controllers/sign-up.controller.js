angular.module('appModule')
    .controller('SignUpController', ['$scope', '$controller', 'AccountService', 'Navigator',
        function ($scope, $controller, accountService, navigator) {
            $controller('BaseController', {
                $scope: $scope
            });

            $scope.signUpText = "Sign Up";
            $scope.cancelText = "Sign In";

            $scope.signUpModel = {
                userName: "",
                password: "",
                confirmPassword: ""
            };

            $scope.init = function () {
                if (navigator.isAccountView() === true) {
                    $scope.signUpText = "Add New Account";
                    $scope.cancelText = "Cancel";
                }
            }

            $scope.signUp = function () {
                $scope.clearInfoMessages();

                if (isValidPasswordConfirmation() === false) {
                    $scope.addError("The password and confirmation password do not match.");
                    return;
                }
                
                accountService.signUp($scope.signUpModel)
                    .then(
                        onSuccessfulSignUp,
                        $scope.onFailure);
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
                return (
                    $scope.signUpModel.password ===
                    $scope.signUpModel.confirmPassword);
            }

            function onSuccessfulSignUp () {
                $scope.toastSuccess("User has been successfully signed up.");
                $scope.goBack();
            }
        }
    ]);