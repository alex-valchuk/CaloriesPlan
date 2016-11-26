angular.module('appModule')
    .service('MealService', ['$http',
        function ($http) {
            this.baseUrl = "http://localhost/CaloriesPlan/api/meals/";

            var unableToContactServer = "Unable to contact server; please, try again later.";

            this.headers = {
                json: {
                    'Content-Type': 'application/json'
                },
                form: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }

            function leadingZeros(number, requiredLength) {
                return (number.toString().length < requiredLength)
                    ? leadingZeros("0" + number, requiredLength)
                    : number;
            }

            this.getNutritionReport = function (userName, filter) {
                var request = {
                    method: "GET",
                    url: this.baseUrl +
                        "?userName=" + userName +
                        "&dateFrom=" + filter.dateFrom.toISOString() +
                        "&dateTo=" + filter.dateTo.toISOString() +
                        "&timeFrom=" + filter.timeFrom.toISOString() +
                        "&timeTo=" + filter.timeTo.toISOString() +
                        "&pageSize=" + filter.pageSize +
                        "&page=" + filter.page,
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.getMeal = function (userName, mealID) {
                var request = {
                    method: "GET",
                    url: this.baseUrl + userName + "/meal/" + mealID,
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.saveMeal = function (userName, meal, successCallback, failureCallback) {
                var apiMethod = (meal.id > 0) ? "PUT" : "POST";
                var apiUrl = this.baseUrl + userName + "/meal/";
                if (meal.id > 0) {
                    apiUrl += meal.id;
                }

                var request = {
                    method: apiMethod,
                    url: apiUrl,
                    headers: this.headers.json,
                    data: meal
                };

                var promise = $http(request);
                return promise;
            }

            this.deleteMeal = function (userName, mealID) {
                var request = {
                    method: "DELETE",
                    url: this.baseUrl + userName + "/meal/" + mealID,
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }
        }
    ]);