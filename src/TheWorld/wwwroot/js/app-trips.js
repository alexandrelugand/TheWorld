// app-trip.js
(function() {

    "use strict";

    // Creation module
    angular.module("app-trips", ["simpleControls", "ngRoute"])
        .config(function($routeProvider) {

            $routeProvider
                .when("/", {
                    controller: "tripsController",
                    controllerAs: "vm",
                    templateUrl: "/views/tripsView.html"
                })
                .when("/editor/:tripName", {
                    controller: "tripEditorController",
                    controllerAs: "vm",
                    templateUrl: "/views/tripEditorView.html"
                })
                .otherwise({ redirectTo: "/" });

        });

})();