﻿angular.module('appModule')
    .service('Navigator', ['$window', '$location',
        function ($window, $location) {
            this.goToSignIn = function () {
                goTo("signin");
            }

            this.goToNutritionReport = function (userName) {
                goTo("nutrition-report/" + userName);
            }

            this.goToAccountsList = function () {
                goTo("accounts-list");
            }

            this.goToEditRoles = function (userName) {
                goTo("account/" + userName + "/user-roles");
            }

            this.goToMeal = function (userName, mealID) {
                goTo("nutrition-report/" + userName + "/meal/" + mealID);
            }

            this.goBack = function () {
                $window.history.back();
            }

            this.isAccountView = function () {
                return isView('account');
            }

            function isView(view) {
                return ($location.path().includes(view) === true);
            }

            function goTo(view) {
                var url = '#/' + view;
                $window.location.href = url;
            }
        }
    ]);
