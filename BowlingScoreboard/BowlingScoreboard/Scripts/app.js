; (function () {
    'use strict';
    var app = angular.module('BowlingScoreboard.Website', [])
        .value('version', '1.0.0.0')

    app.FrameObject = function (srno, roll1, roll2, roll3, strike, spare, totalscore) {
        this.SrNo = srno;
        this.FirstRoll = roll1;
        this.SecondRoll = roll2;
        this.ThirdRoll = roll3;        
        this.Strike = strike;
        this.Spare = spare;
        this.TotalScore = totalscore;
    };

    app.directive('parentContainer', function ($rootScope, $templateRequest, $compile, $http, $window) {
        return {
            restrict: "C",
            scope: true,
            link: function (scope, element, attrs) {
                var jsonAppConfigStr = angular.fromJson(attrs.jsonAppConfig !== undefined ? attrs.jsonAppConfig : '{}');
                scope.addFrameButtonText = jsonAppConfigStr.labels.addframebuttontext;

                scope.frames = [];
                scope.showAddFrame = true;
                scope.showCalculateButton = false;
                scope.disableAddFrame = false;
                scope.Strike = false;
                scope.Spare = false;
                scope.finalScore = 0;

                var frameCount = 0;
                var tbody = angular.element(element)[0].querySelector('.table-body');

                scope.addFrameRowToScoreTable = function () {
                    scope.disableAddFrame = true;
                    if (frameCount < 10) {
                        $templateRequest("../../Scripts/frame-row.tpl.html").then(function (html) {
                            var fullhtml = angular.element(html)[0];
                            fullhtml.cells[0].innerHTML = '<div class="frame-number">' + frameCount + '</div>';
                            if (frameCount <= 9) {
                                fullhtml.cells[3].innerHTML = '';
                            }                            
                            angular.element(tbody).append($compile(fullhtml)(scope));
                        });
                        frameCount += 1;
                    }
                    if (frameCount === 10) {
                        scope.showAddFrame = false;
                        scope.disableAddFrame = true;
                    }
                };

                scope.readAndProcessScore = function () {
                    var button = angular.element(event.target)[0];
                    readScoreFromFields(button);
                };

                /*
                Shows final result(total score) of all frames
                */
                scope.showResult = function () {
                    scope.finalScore = scope.frames[scope.frames.length - 1].TotalScore;
                };

                /*
                ***Reads the score from the dropdowns. 
                ***Pass it to the Web API for calculation.
                ***Store the output in an scope array
                */
                var readScoreFromFields = function (button) {
                    var roll1 = angular.element(element)[0].querySelector('#roll1');
                    var roll2 = angular.element(element)[0].querySelector('#roll2');
                    var roll3 = angular.element(element)[0].querySelector('#roll3');
                    var roll1value = parseInt(roll1.value);
                    var roll2value = parseInt(roll2.value);
                    var roll3value = 0;

                    if (!inputValidationAndCategoryIdentification(roll1value, roll2value, button)) {
                        return;
                    }

                    if (typeof roll3 !== "undefined" && roll3 !== null && roll1value + roll2value >= 10) {
                        roll3value = parseInt(roll3.value);
                    }

                    var scoreServiceUrl = "http://localhost:59595/api/framescore?firstrollpins="
                        + roll1value
                        + "&secondrollpins=" + roll2value
                        + "&thirdrollpins=" + roll3value
                        + "&totalscore=" + ((scope.frames.length > 0) ? scope.frames[scope.frames.length - 1].TotalScore.toString() : "0")
                        + "&strike=" + ((scope.frames.length > 0) ? scope.frames[scope.frames.length - 1].Strike.toString() : "false")
                        + "&spare=" + ((scope.frames.length > 0) ? scope.frames[scope.frames.length - 1].Spare.toString() : "false")
                        + "&pstrike=" + ((scope.frames.length > 1) ? scope.frames[scope.frames.length - 2].Strike.toString() : "false")
                        + "&pspare=" + ((scope.frames.length > 1) ? scope.frames[scope.frames.length - 2].Spare.toString() : "false")
                        + "&framenum=" + frameCount;

                    $http.get(scoreServiceUrl).then(function (response) {
                        var jsonResponse = JSON.parse(response.data);
                        var frameScore = new app.FrameObject(frameCount,
                            jsonResponse.FirstRollPinsCount,
                            jsonResponse.SecondRollPinsCount,
                            jsonResponse.ThirdRollPinsCount,
                            scope.Strike,
                            scope.Spare,
                            jsonResponse.TotalScore);

                        button.parentElement.parentElement.cells[4].innerHTML = frameScore.TotalScore;
                        if (scope.frames.length > 0) {
                            if (scope.frames[scope.frames.length - 1].Strike === true || scope.frames[scope.frames.length - 1].Spare === true) {
                                var previousFrame = button.parentElement.parentElement.previousSibling;
                                if (previousFrame !== null) {
                                    previousFrame.cells[4].innerHTML = frameScore.TotalScore - roll1value - roll2value;
                                }
                            }
                        }
                        if (scope.frames.length > 1) {
                            if (scope.frames[scope.frames.length - 2].Strike === true || scope.frames[scope.frames.length - 2].Spare === true) {
                                var prevToPrevFrame = button.parentElement.parentElement.previousSibling.previousSibling;
                                if (prevToPrevFrame !== null) {
                                    prevToPrevFrame.cells[4].innerHTML = (frameScore.TotalScore - roll1value - roll2value) - roll1value - roll2value - 10;
                                }
                            }
                        }
                                                
                        scope.frames.push(frameScore);
                        scope.disableAddFrame = false;
                        scope.Strike = false;
                        scope.Spare = false;

                        replaceDropdowns(roll1, roll2, roll3);

                        if (frameCount === 10) {
                            scope.showCalculateButton = true;
                        }
                    });
                };

                /*
                Validate inputs and show error messages
                Checks whether the rolls are a Strike or Spare
                */
                var inputValidationAndCategoryIdentification = function (roll1value, roll2value, button) {
                    if (roll1value + roll2value > 10 && frameCount !== 10) {
                        scope.errorVisible = true;
                        angular.element(element)[0].querySelector('.error-message').innerHTML = 'Invalid Score Entered';
                        return false;
                    } else {
                        scope.errorVisible = false;
                        button.disabled = true;
                    }

                    if (roll1value !== 10 && roll2value !== 10 && ((roll1value + roll2value) === 10)) {
                        scope.Spare = true;
                    }
                    if (roll1value === 10) {
                        scope.Strike = true;
                    }
                    return true;
                };

                /*
                Replace the dropdowns with the static divs
                */
                var replaceDropdowns = function (roll1, roll2, roll3) {
                    roll1.parentElement.innerHTML = '<div>' + roll1.value + '</div>'
                    roll2.parentElement.innerHTML = '<div>' + roll2.value + '</div>';
                    if (typeof roll3 !== "undefined" && roll3 !== null) {
                        roll3.parentElement.innerHTML = '<div>' + roll3.value + '</div>';
                    }
                };

                /*
                Starts a new game with a fresh scoreboard
                */
                $rootScope.startNewGame = function () {
                    if ($window.confirm("Are you sure you want to start new game?")) {
                        $window.location.reload();
                    }
                };
            }
        };
    });
})();