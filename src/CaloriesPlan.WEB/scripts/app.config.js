angular.module('appModule')
  .config(['$routeProvider',
    function config($routeProvider) {

        $routeProvider
          .when('/',
              {
                  redirectTo: '/signin'
              })
          .when('/signin',
              {
                  templateUrl: 'scripts/views/sign-in.html'
              })
          .when('/signup',
              {
                  templateUrl: 'scripts/views/sign-up.html'
              })
          .when('/nutrition-report/:userName',
              {
                  templateUrl: 'scripts/views/nutrition-report.html'
              })
          .when('/nutrition-report/:userName/meal/:mealID?',
              {
                  templateUrl: 'scripts/views/meal.html'
              })
          .when('/accounts-list',
              {
                  templateUrl: 'scripts/views/accounts-list.html'
              })
          .when('/account',
              {
                  templateUrl: 'scripts/views/sign-up.html'
              })
          .when('/account/:userName',
              {
                  templateUrl: 'scripts/views/account.html'
              })
          .when('/account/:userName/user-roles',
              {
                  templateUrl: 'scripts/views/user-roles.html'
              })
          .when('/friends-list/:userID?',
              {
                  templateUrl: 'scripts/views/friends-list.html'
              })
          .when('/tests',
              {
                  templateUrl: 'scripts/views/tests.html'
              })
          .otherwise({ redirectTo: '/' });
    }
  ]);