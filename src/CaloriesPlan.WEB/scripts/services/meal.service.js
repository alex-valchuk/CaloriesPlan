angular.module('appModule')
    .service('MealService', ['$http',
        function ($http) {
            this.baseUrl = "http://localhost/CaloriesPlan/api/meals/";

            var unableToContactServer = "Unable to contact server; please, try again later.";

            this.jsonConfig = {
                headers: {
                    'Content-Type': 'application/json'
                }
            }

            this.getNutritionReport = function (userName, filter, successCallback, failureCallback) {
                var request = {
                    method: "POST",
                    url: this.baseUrl + userName,
                    headers: this.jsonConfig.headers,
                    data: filter
                };

                $http(request)
                    .success(function (data) {
                        if (typeof successCallback === 'function') {
                            successCallback(data);
                        }
                    })
                    .error(function (data, code) {
                        if (typeof failureCallback === 'function') {
                            failureCallback(data, code);
                        }
                    });
            }

            this.getMeal = function (userName, mealID, successCallback, failureCallback) {
                var request = {
                    method: "GET",
                    url: this.baseUrl + userName + "/meal/" + mealID,
                    headers: this.jsonConfig.headers
                };

                $http(request)
                    .success(function (data) {
                        if (typeof successCallback === 'function') {
                            successCallback(data);
                        }
                    })
                    .error(function (data, code) {
                        if (typeof failureCallback === 'function') {
                            failureCallback(data, code);
                        }
                    });
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
                    headers: this.jsonConfig.headers,
                    data: meal
                };

                $http(request)
                    .success(function (data) {
                        if (typeof successCallback === 'function') {
                            successCallback(data);
                        }
                    })
                    .error(function (data, code) {
                        if (typeof failureCallback === 'function') {
                            failureCallback(data, code);
                        }
                    });
            }

            this.deleteMeal = function (userName, mealID, successCallback, failureCallback) {
                var request = {
                    method: "DELETE",
                    url: this.baseUrl + userName + "/meal/" + mealID,
                    headers: this.jsonConfig.headers
                };

                $http(request)
                    .success(function (data) {
                        if (typeof successCallback === 'function') {
                            successCallback(data);
                        }
                    })
                    .error(function (data, code) {
                        if (typeof failureCallback === 'function') {
                            failureCallback(data, code);
                        }
                    });
            }
        }
    ]);