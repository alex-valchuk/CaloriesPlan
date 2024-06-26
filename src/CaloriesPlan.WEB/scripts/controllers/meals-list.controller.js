﻿angular.module('appModule')
    .controller('MealsListController', ['$scope', '$controller', '$routeParams', '$filter', 'MealService', 'AccountService', 'Navigator',
        function ($scope, $controller, $routeParams, $filter, mealService, accountService, navigator) {
            $controller('BaseSecuredController', {
                $scope: $scope
            });

            $scope.init = function () {
                $scope.authenticate();

                setupDateRanges();
                setupTimeRanges();
                $scope.fillForm();
            }

            $scope.filter = {
                pageSize: 5,
                page: 0
            };

            function setupDateRanges () {
                var date = new Date(Date.now());

                var yesterday = new Date(Date.now());
                yesterday.setDate(yesterday.getDate() - 1);

                var currentMonth = new Date(Date.now());
                
                $scope.dateRange = {
                    availableOptions: [
                      {
                          value: '0', name: 'Current Year',
                          dateFrom: new Date(date.getFullYear(), 0, 1),
                          dateTo: new Date(date.getFullYear(), 11, 31)
                      },
                      {
                          value: '1', name: 'Today',
                          dateFrom: new Date(date.getFullYear(), date.getMonth(), date.getDate()),
                          dateTo: new Date(date.getFullYear(), date.getMonth(), date.getDate()) 
                      },
                      {
                          value: '2', name: 'Yesterday',
                          dateFrom: new Date(yesterday.getFullYear(), yesterday.getMonth(), yesterday.getDate()),
                          dateTo: new Date(yesterday.getFullYear(), yesterday.getMonth(), yesterday.getDate())
                      },
                      {
                          value: '3', name: 'Current Month',
                          dateFrom: new Date(currentMonth.getFullYear(), currentMonth.getMonth(), 1),
                          dateTo: new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 0)
                      },
                      {
                          value: '4', name: 'Last Month',
                          dateFrom: new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1, 1),
                          dateTo: new Date(currentMonth.getFullYear(), currentMonth.getMonth(), 0)
                      }
                    ]
                };

                $scope.onDateRangeChanged(0);
            }

            $scope.onDateRangeChanged = function (index) {
                var selectedOption = $scope.dateRange.availableOptions[index];
                $scope.dateRange.selectedValue = selectedOption.value;

                $scope.filter.dateFrom = selectedOption.dateFrom;
                $scope.filter.dateTo = selectedOption.dateTo;
            }

            function setupTimeRanges() {
                var year = new Date().getFullYear();

                $scope.timeRange = {
                    availableOptions: [
                      {
                          value: '0', name: 'Whole Day',
                          timeFrom: new Date(year, 0, 1, 0, 0, 0),
                          timeTo: new Date(year, 0, 1, 23, 59, 59)
                      },
                      {
                          value: '1', name: 'Breakfast',
                          timeFrom: new Date(year, 0, 1, 7, 0, 0),
                          timeTo: new Date(year, 0, 1, 8, 0, 0)
                      },
                      {
                          value: '2', name: 'Lunch',
                          timeFrom: new Date(year, 0, 1, 13, 0, 0),
                          timeTo: new Date(year, 0, 1, 14, 0, 0)
                      },
                      {
                          value: '3', name: 'Dinner',
                          timeFrom: new Date(year, 0, 1, 18, 0, 0),
                          timeTo: new Date(year, 0, 1, 19, 0, 0)
                      }
                    ]
                };
                
                $scope.onTimeRangeChanged(0);
            }

            $scope.onTimeRangeChanged = function (index) {
                var selectedOption = $scope.timeRange.availableOptions[index];
                $scope.timeRange.selectedValue = selectedOption.value;

                $scope.filter.timeFrom = selectedOption.timeFrom;
                $scope.filter.timeTo = selectedOption.timeTo;
            }

            $scope.toLocaleDate = function (e) {
                e.date = $filter('utcToLocal')(e.eatingDate, 'yyyy-MM-dd');
                return e;
            };

            $scope.getTotalCalories = function (meals) {
                return meals
                    .map(function (meal) { return meal.calories; })
                    .reduce(function (a, b) { return a + b; });
            };

            $scope.fillForm = function () {
                mealService.getMeals($routeParams.userName, $scope.filter)
                    .then(
                        onSuccessfulGettingNutritionReport,
                        $scope.onFailure);
            }

            function onSuccessfulGettingNutritionReport(response) {
                $scope.nutritionReport = response.data;
            }

            $scope.editMeal = function (mealID) {
                navigator.goToMeal($routeParams.userName, mealID);
            }

            $scope.deleteMeal = function (mealID) {
                mealService.deleteMeal(mealID)
                    .then(
                        onSuccessfulDeletingMeal,
                        $scope.onFailure);
            }

            function onSuccessfulDeletingMeal() {
                $scope.toastSuccess("Meal has been successfully deleted.");
                $scope.fillForm();
            }
        }
    ]);