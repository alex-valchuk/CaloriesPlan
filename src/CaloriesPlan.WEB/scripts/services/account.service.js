angular.module('appModule')
    .service('AccountService', ['$http', '$cookieStore', '$q',
        function ($http, $cookieStore, $q) {
            this.baseUrl = "http://localhost/CaloriesPlan/api/accounts/";

            var userData = {
                isAuthenticated: false,
                userName: '',
                bearerToken: '',
                expirationDate: null,
                roles: []
            };

            this.headers = {
                json: {
                    'Content-Type': 'application/json'
                },
                form: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }

            this.signUp = function (signUpModel) {
                var request = {
                    method: 'POST',
                    url: this.baseUrl + "signup",
                    headers: this.headers.json,
                    data: signUpModel
                };

                var promise = $http(request);
                return promise;
            };

            this.signIn = function (signInModel) {
                var request = {
                    method: 'POST',
                    url: this.baseUrl + "signin",
                    headers: this.headers.form,
                    data: 'grant_type=password&userName=' + signInModel.userName + '&password=' + signInModel.password,
                };

                var promise = $http(request);
                promise.then(
                    /* succeess */
                    function (response) {
                        var data = response.data;

                        userData.isAuthenticated = true;
                        userData.userName = data.userName;
                        userData.bearerToken = data.access_token;
                        userData.expirationDate = Date.now() + data.expires_in * 1000;
                        userData.roles = angular.fromJson(data.roles);

                        setHttpAuthHeader();
                        saveAuthData();
                    });
                
                return promise;
            };

            this.signOut = function () {
                var request = {
                    method: 'POST',
                    url: this.baseUrl + "signout",
                };

                var promise = $http(request);
                promise.then(
                    /* succeess */
                    function (response) {
                        clearHttpAuthHeader();
                        clearAuthData();
                        clearUserData();
                    });

                return promise;
            }

            this.isAuthenticated = function () {
                if (userData.isAuthenticated && !isAuthenticationExpired(userData.expirationDate)) {
                    return true;
                } else {
                    try {
                        retrieveAuthData();
                    } catch (e) {
                        return false;
                    }

                    return true;
                }
            };

            this.getUserName = function () {
                if (this.isAuthenticated()) {
                    return userData.userName;
                }

                return "";
            }

            this.getAccounts = function () {
                var request = {
                    method: 'GET',
                    url: this.baseUrl,
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.getAccount = function (userName) {
                var request = {
                    method: 'GET',
                    url: this.baseUrl + userName,
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.saveAccount = function (accountModel) {
                var request = {
                    method: 'PUT',
                    url: this.baseUrl + accountModel.userName,
                    headers: this.headers.json,
                    data: {
                        "dailyCaloriesLimit": accountModel.dailyCaloriesLimit
                    }
                };
                
                var promise = $http(request);
                return promise;
            }

            this.deleteAccount = function (userName) {
                var request = {
                    method: "DELETE",
                    url: this.baseUrl + userName,
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.getUserRoles = function (userName) {
                var request = {
                    method: "GET",
                    url: this.baseUrl + userName + "/user-roles",
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.getNonUserRoles = function (userName) {
                var request = {
                    method: "GET",
                    url: this.baseUrl + userName + "/not-user-roles",
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.addUserRole = function (userName, roleName) {
                var request = {
                    method: "POST",
                    url: this.baseUrl + userName + "/user-roles/" + roleName,
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.deleteUserRole = function (userName, roleName) {
                var request = {
                    method: "DELETE",
                    url: this.baseUrl + userName + "/user-roles/" + roleName,
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.getSubscribers = function (userName) {
                var request = {
                    method: "GET",
                    url: this.baseUrl + userName + "/subscribers",
                    headers: this.headers.json
                };

                var promise = $http(request);
                return promise;
            }

            this.isUserHasRoles = function (requiredRoles) {
                if (userData.roles &&
                    requiredRoles &&
                    requiredRoles.length > 0) {
                    for (var i = 0; i < requiredRoles.length; i++) {

                        var requiredRole = requiredRoles[i];
                        if (userData.roles.indexOf(requiredRole) >= 0) {
                            return true;
                        }
                    }
                }
                    
                return false;
            }

            function NoAuthenticationException(message) {
                this.name = 'AuthenticationRequired';
                this.message = message;
            }

            function AuthenticationExpiredException(message) {
                this.name = 'AuthenticationExpired';
                this.message = message;
            }

            function AuthenticationRetrievalException(message) {
                this.name = 'AuthenticationRetrieval';
                this.message = message;
            }

            function isAuthenticationExpired(expirationDate) {
                var now = new Date();
                var expDate = new Date(expirationDate);
                if (expDate - now > 0) {
                    return false;
                } else {
                    return true;
                }
            }

            function saveAuthData() {
                clearAuthData();
                $cookieStore.put('auth_data', userData);
            }

            function clearAuthData() {
                $cookieStore.remove('auth_data');
            }

            function retrieveAuthData() {
                var savedData = $cookieStore.get('auth_data');
                if (typeof savedData === 'undefined') {

                    throw new AuthenticationRetrievalException('No authentication data exists');
                } else if (isAuthenticationExpired(savedData.expirationDate)) {

                    throw new AuthenticationExpiredException('Authentication token has already expired');
                } else {

                    userData = savedData;
                    setHttpAuthHeader();
                }
            }

            function setHttpAuthHeader() {
                $http.defaults.headers.common.Authorization = 'Bearer ' + userData.bearerToken;
            }

            function clearHttpAuthHeader() {
                $http.defaults.headers.common.Authorization = '';
            }

            function clearUserData() {
                userData.isAuthenticated = false;
                userData.userName = '';
                userData.bearerToken = '';
                userData.expirationDate = null;
                userData.roles = null;
            }
        }
    ]);