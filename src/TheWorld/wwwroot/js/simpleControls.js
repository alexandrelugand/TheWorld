// simpleControls.js

(function () {

    angular.module("simpleControls", [])
        .directive("waitCursor", function () {

            return {
                scope: {
                    show: "=displayWhen"   
                },
                restrict: "E",
                templateUrl: "/views/waitCursor.html"
            }

        });

})();