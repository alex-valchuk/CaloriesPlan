appModule
  .component('myMenu', {
      templateUrl: 'scripts/views/menu.html',
      controller: ['$scope', '$controller', 'Navigator', 'AccountService', MenuController]
    });

function MenuController($scope, $controller, navigator, accountService) {
    $controller('BaseSecuredController', {
        $scope: $scope
    });

    this.$onInit = function () {
        $scope.refresh();
    };

    $scope.refresh = function () {//calling from login as well
        $scope.authenticate();//calling base method
    }

    $scope.signout = function () {
        $scope.clearInfoMessages();

        accountService.signout(onSuccessfullSignout, onFailedSignout);
    }

    function onSuccessfullSignout() {
        $scope.refresh();

        if (navigator) {
            navigator.goToLogin();
        }
    }

    function onFailedSignout(data, code) {
        $scope.commonFailureCallback(data, code);
    }
}