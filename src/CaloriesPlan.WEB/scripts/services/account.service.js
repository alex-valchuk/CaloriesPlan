angular.module('appModule')
    .service('AccountService', ['$http', '$cookieStore',
        function ($http, $cookieStore) {
            this.baseUrl = "http://localhost/CaloriesPlan/api/accounts/";

            var userData = {
                isAuthenticated: false,
                userName: '',
                bearerToken: '',
                expirationDate: null,
                roles: []
            };

            this.jsonConfig = {
                headers: {
                    'Content-Type': 'application/json'
                }
            }

            this.register = function (userName, password, confirmPassword, successCallback, failureCallback) {
                var registerModel = {
                    "UserName": userName,
                    "Password": password,
                    "ConfirmPassword": confirmPassword
                };

                var url = this.baseUrl + "register";

                $http.post(url, registerModel, this.jsonConfig)
                    .success(function (data) {
                        if (typeof successCallback === 'function') {
                            successCallback();
                        }
                    })
                    .error(function (data, code) {                        
                        if (typeof failureCallback === 'function') {
                            failureCallback(data, code);
                        }
                    });
            };

            this.login = function (userName, password, successCallback, failureCallback) {
                var request = {
                    method: 'POST',
                    url: this.baseUrl + "login",
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                    },
                    data: 'grant_type=password&userName=' + userName + '&password=' + password,
                };

                $http(request)
                    .success(function (data, val) {
                        userData.isAuthenticated = true;
                        userData.userName = data.userName;
                        userData.bearerToken = data.access_token;
                        userData.expirationDate = Date.now() + data.expires_in * 1000;
                        userData.roles = angular.fromJson(data.roles);

                        setHttpAuthHeader();
                        saveAuthData();

                        if (typeof successCallback === 'function') {
                            successCallback();
                        }
                    })
                    .error(function (data, code) {
                        if (typeof failureCallback === 'function') {
                            failureCallback(data, code);
                        }
                    });
            };

            this.signout = function (successCallback, failureCallback) {
                try {
                    clearHttpAuthHeader();
                    clearAuthData();
                    clearUserData();

                    if (typeof successCallback === 'function') {
                        successCallback();
                    }
                } catch (ex) {
                    if (typeof failureCallback === 'function') {
                        failureCallback(ex.message);
                    }
                }
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

            this.getAccounts = function (successCallback, failureCallback) {
                var request = {
                    method: 'GET',
                    url: this.baseUrl,
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

            this.getAccount = function (userName, successCallback, failureCallback) {
                var request = {
                    method: 'GET',
                    url: this.baseUrl + userName,
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

            this.saveAccount = function (userName, dailyCaloriesLimit, successCallback, failureCallback) {
                var request = {
                    method: 'PUT',
                    url: this.baseUrl + userName,
                    headers: this.jsonConfig.headers,
                    data: {
                        "dailyCaloriesLimit": dailyCaloriesLimit
                    }
                };
                
                $http(request)
                    .success(function (data) {
                        if (typeof successCallback === 'function') {
                            successCallback();
                        }
                    })
                    .error(function (data, code) {
                        if (typeof failureCallback === 'function') {
                            failureCallback(data, code);
                        }
                    });
            }

            this.deleteAccount = function (userName, successCallback, failureCallback) {
                var request = {
                    method: "DELETE",
                    url: this.baseUrl + userName,
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

            this.getUserRoles = function (userName, successCallback, failureCallback) {
                var request = {
                    method: "GET",
                    url: this.baseUrl + userName + "/user-roles",
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

            this.getNotUserRoles = function (userName, successCallback, failureCallback) {
                var request = {
                    method: "GET",
                    url: this.baseUrl + userName + "/not-user-roles",
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

            this.addUserRole = function (userName, roleName, successCallback, failureCallback) {
                var request = {
                    method: "POST",
                    url: this.baseUrl + userName + "/user-roles/" + roleName,
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

            this.deleteUserRole = function (userName, roleName, successCallback, failureCallback) {
                var request = {
                    method: "DELETE",
                    url: this.baseUrl + userName + "/user-roles/" + roleName,
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