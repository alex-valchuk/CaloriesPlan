angular.module('appModule')
    .controller('MealController', ['$scope', '$controller', '$routeParams', '$filter', 'MealService', 'Navigator',
        function ($scope, $controller, $routeParams, $filter, mealService, navigator) {
            $controller('BaseSecuredController', {
                $scope: $scope
            });

            $scope.meal = {
                id: 0,
                text: "",
                calories: "",
                eatingDate: new Date(Date.now())
            };

            $scope.init = function () {
                $scope.authenticate();

                fillForm();
            }

            function fillForm() {
                var userName = $routeParams.userName;
                var mealID = $routeParams.mealID;
                if (mealID > 0) {
                    $scope.mealOperationName = "Edit Meal";

                    mealService.getMeal(userName, mealID)
                        .then(
                            onSuccessfulGettingMeal,
                            $scope.onFailure);
                } else {
                    $scope.mealOperationName = "Add Meal";
                }

            }

            function onSuccessfulGettingMeal(response) {
                var data = response.data;

                $scope.meal.id = data.id;
                $scope.meal.text = data.text;
                $scope.meal.calories = data.calories;
                $scope.meal.eatingDate = new Date(data.eatingDate)
            }

            $scope.save = function () {
                $scope.clearInfoMessages();

                mealService.saveMeal($routeParams.userName, $scope.meal)
                    .then(
                        onSuccessfulSavingMeal,
                        $scope.onFailure);
            }

            function onSuccessfulSavingMeal () {
                $scope.toastSuccess("Meal has been successfully saved.");
                navigator.goBack();
            }
        }
    ]);