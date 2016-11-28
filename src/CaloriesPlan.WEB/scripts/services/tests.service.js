angular.module('appModule')
    .service('TestsService', ['$http', '$cookieStore', '$q',
        function ($http, $cookieStore, $q) {
            this.baseUrl = "http://localhost/CaloriesPlan/api/tests/";

            this.headers = {
                json: {
                    'Content-Type': 'application/json'
                },
                form: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }

            this.getUsers = function () {
                var request = {
                    method: 'GET',
                    url: this.baseUrl + "friends_w",
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.getFriends = function (userName) {
                var request = {
                    method: "GET",
                    url: this.baseUrl + userName + "/friends",
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }
        }
    ]);