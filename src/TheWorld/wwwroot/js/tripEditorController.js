// tripEditorController.js

(function() {

    "use strict";

    angular.module("app-trips")
        .controller("tripEditorController", function ($routeParams, $http) {

            var vm = this;
            vm.tripName = $routeParams.tripName;
            vm.stops = [];
            vm.errorMessage = "";
            vm.isBusy = true;
            vm.newStop = {};
            var url = "/api/trips/" + vm.tripName + "/stops";

            $http.get(url)
                .then(function(response) {
                    // Success
                    angular.copy(response.data, vm.stops);
                    _showMaps(vm.stops);
                }, function(error) {
                    // Error
                    vm.errorMessage = "Failed to load stops. " + error.data;
                })
                .finally(function() {
                    vm.isBusy = false;
                });

            vm.addStop = function () {

                vm.isBusy = true;
                vm.errorMessage = "";

                $http.post(url, vm.newStop)
                    .then(function (response) {
                        // Success
                        vm.stops.push(response.data);
                        _showMaps(vm.stops);
                        vm.newStop = {};
                    }, function (error) {
                        // Error
                        vm.errorMessage = "Failed to save new stop. " + error.data;
                    })
                    .finally(function () {
                        vm.isBusy = false;
                    });

            };

        });

    function _showMaps(stops) {

        if (stops && stops.length > 0) {

            var mapStops = _.map(stops, function(item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });

            // Show Map
            travelMap.createMap({
                stops: mapStops,
                selector: "#map",
                currentStop: 0,
                icon: {           // Icon details
                    url: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAYAAAAGCAIAAABvrngfAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAadEVYdFNvZnR3YXJlAFBhaW50Lk5FVCB2My41LjExR/NCNwAAAA1JREFUGFdjoDNgYAAAAHIAAejt7scAAAAASUVORK5CYII=",
                    width: 3,
                    height: 3,
                },
                initialZoom: 2,   // Initial Level of Zoom for the Google Map
                pastStroke: {     // Settings for the lines before the currentStop
                    color: '#190300',
                    opacity: 0.5,
                    weight: 2
                },
                futureStroke: {   // Settings for hte lines after the currentStop
                    color: '#D30000',
                    opacity: 0.6,
                    weight: 2
                },
                mapOptions: {     // Options for map (See GMaps for full list of options)
                    draggable: true,
                    scrollwheel: false,
                    disableDoubleClickZoom: true,
                    zoomControl: true
                }
            });

        }

    }

})();