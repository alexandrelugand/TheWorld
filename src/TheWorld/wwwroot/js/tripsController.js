// tripsController.js

(function() {

    "use strict";

    angular.module("app-trips")
        .controller("tripsController", function ($http) {

        var vm = this;
        vm.trips = [];   
        vm.newTrip = {};
        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/trips")
            .then(function(response) {
                // Success
                angular.copy(response.data, vm.trips);
            }, function(error) {
                // Failure
                vm.errorMessage = "Failed to load trips. " + error.data;
            })
            .finally(function() {
                vm.isBusy = false;
            });

        vm.addTrip = function () {

            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/trips", vm.newTrip)
                .then(function(response) {
                    // Success
                    vm.trips.push(response.data);
                    vm.newTrip = {};
                }, function(error) {
                    // Failure
                    vm.errorMessage = "Failed to save new trip. " + error.data;
                })
                .finally(function() {
                    vm.isBusy = false;
                });

        };

    });

})();