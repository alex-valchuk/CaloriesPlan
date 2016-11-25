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

    $scope.refresh = function () {//calling from signin as well
        $scope.authenticate();//calling base method
    }

    $scope.signOut = function () {
        $scope.clearInfoMessages();

        accountService.signOut()
            .then(
                onSuccessfulSignOut,
                $scope.onFailure);
    }

    function onSuccessfulSignOut() {
        $scope.refresh();

        if (navigator) {
            navigator.goToSignIn();
        }
    }
}