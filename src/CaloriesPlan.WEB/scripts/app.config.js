angular.module('appModule')
  .config(['$routeProvider',
    function config($routeProvider) {

        $routeProvider
          .when('/',
              {
                  redirectTo: '/login'
              })
          .when('/login',
              {
                  templateUrl: 'scripts/views/login.html'
              })
          .when('/register',
              {
                  templateUrl: 'scripts/views/register.html'
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
                  templateUrl: 'scripts/views/register.html'
              })
          .when('/account/:userName',
              {
                  templateUrl: 'scripts/views/account.html'
              })
          .when('/account/:userName/user-roles',
              {
                  templateUrl: 'scripts/views/user-roles.html'
              })
          .otherwise({ redirectTo: '/' });
    }
  ]);