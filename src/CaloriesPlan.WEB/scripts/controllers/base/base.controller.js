angular.module('appModule')
    .controller('BaseController', ['$scope', 'ngToast', 'Navigator',
        function ($scope, ngToast, navigator) {
            $scope.unableToContactServer = "Unable to contact server; please, try again later.";
            $scope.notAvailableResource = "Requested resource is not available.";
            $scope.internalServerError = "Internal server error.";
            $scope.notFound = "The resource you are trying to get is not found.";

            $scope.errors = [];

            $scope.toastSuccess = function (message) {
                ngToast.create({
                    className: 'success',
                    content: message,
                    horizontalPosition: 'center'
                });
            }

            $scope.toastFailure = function (message) {
                ngToast.create({
                    className: 'danger',
                    content: message,
                    horizontalPosition: 'center'
                });
            }

            $scope.onFailure = function (error) {
                if (error) {
                    var data = error.data;
                    var code = error.status;

                    if (code) {
                        switch (code) {
                            case 401:
                                if (data.message) {
                                    $scope.toastFailure(data.message);
                                }

                                navigator.goToSignIn();
                                break;

                            case 403:
                                if (data.message) {
                                    $scope.toastFailure(data.message);
                                } else {
                                    $scope.toastFailure($scope.notAvailableResource);
                                }

                                navigator.goBack();
                                break;

                            case 404:
                                $scope.toastFailure($scope.notFound);

                                navigator.goBack();
                                break;

                            default:
                                if (data) {
                                    if (data.modelState) {
                                        $scope.parseModelErrors(data.modelState);
                                        return;

                                    } else if (data.error_description) {
                                        $scope.addError(data.error_description);
                                        return;

                                    } else if (data.message) {
                                        $scope.addError(data.message);
                                        return;
                                    }
                                }

                                $scope.addError($scope.internalServerError);
                                break;
                        }
                    }
                    else {
                        $scope.addError($scope.unableToContactServer);
                    }
                } else {
                    $scope.addError($scope.unableToContactServer);
                }
            }

            $scope.parseModelErrors = function (modelState) {
                for (var key in modelState) {
                    for (var i = 0; i < modelState[key].length; i++) {
                        $scope.addError(modelState[key][i]);
                    }
                }
            }

            $scope.addError = function (errorMessage) {
                $scope.errors.push(errorMessage);
            }

            $scope.clearInfoMessages = function () {
                $scope.success = "";
                $scope.errors = [];
            }

            $scope.goBack = function () {
                navigator.goBack();
            }
        }
    ]);
