/// <reference path="../scripts/_references.js" />
/// <reference path="../../TheWorld/wwwroot/js/app-trips.js" />
/// <reference path="../../TheWorld/wwwroot/js/simpleControls.js" />
/// <reference path="../../TheWorld/wwwroot/js/tripsController.js" />

describe("tripsController", function () {


    beforeEach(function () {

        module("app-trips");

    });

    var $httpBackend;

    beforeEach(inject(function($injector) {

        $httpBackend = $injector.get("$httpBackend");

        $httpBackend.when("GET", "/api/trips")
            .respond([
                {
                    id: 1,
                    name: "Trip Test 1",
                    created: "05/01/2016",
                    stops: [
                        {
                            id: 1,
                            name: "Stop Test 1",
                            longitude: 100,
                            latitude: 10,
                            arrival: "05/01/2016",
                            order: 1
                        }
                    ]
                },
                {
                    id: 2,
                    name: "Trip Test 2",
                    created: "05/01/2016",
                    stops: [
                        {
                            id: 2,
                            name: "Stop Test 1",
                            longitude: 100,
                            latitude: 10,
                            arrival: "05/01/2016",
                            order: 1        
                        }
                    ]
                }
            ]);
    }));

    afterEach(function () {

        // Pour que les tests s'exécutent de façon indépendante
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();

    });

    describe("tripsController ", function() {

        it("load data", inject(function ($controller) {

            $httpBackend.expectGET("/api/trips");

            var ctrl = $controller("tripsController");

            $httpBackend.flush();

            expect(ctrl).not.toBeNull();
            expect(ctrl.trips).toBeDefined();

        }));

    });

});