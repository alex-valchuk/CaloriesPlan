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

                var userName = $routeParams.userName;
                var mealID = $routeParams.mealID;
                if (mealID > 0) {
                    $scope.mealOperationName = "Edit Meal";

                    mealService.getMeal(
                        userName,
                        mealID,
                        onSuccessfulGettingMeal,
                        onFailedGettingMeal);
                } else {
                    $scope.mealOperationName = "Add Meal";
                }
            }

            function onSuccessfulGettingMeal(data) {
                $scope.meal.id = data.id;
                $scope.meal.text = data.text;
                $scope.meal.calories = data.calories;
                $scope.meal.eatingDate = new Date(data.eatingDate)
            }

            function onFailedGettingMeal(data, code) {
                $scope.commonFailureCallback(data, code);
            }

            $scope.save = function () {
                $scope.clearInfoMessages();

                mealService.saveMeal(
                    $routeParams.userName,
                    $scope.meal,                    
                    onSuccessfullSavingMeal,
                    onFailedSavingMeal);
            }

            function onSuccessfullSavingMeal (data) {
                $scope.toastSuccess("Meal has been successfully saved.");
                navigator.goBack();
            }

            function onFailedSavingMeal(data, code) {
                $scope.commonFailureCallback(data, code);
            }
        }
    ]);