angular.module('appModule')
    .service('MealService', ['$http', '$routeParams',
        function ($http, $routeParams) {
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

            this.getMeals = function (userName, filter) {
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

            this.getMeal = function (mealID) {
                var request = {
                    method: "GET",
                    url: this.baseUrl + mealID,
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.saveMeal = function (userName, meal) {
                var apiMethod = (meal.id > 0) ? "PUT" : "POST";
                var apiUrl = this.baseUrl;
                if (meal.id > 0) {
                    apiUrl += meal.id;
                } else {
                    apiUrl += "?userName=" + userName;
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

            this.deleteMeal = function (mealID) {
                var request = {
                    method: "DELETE",
                    url: this.baseUrl + mealID,
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }
        }
    ]);