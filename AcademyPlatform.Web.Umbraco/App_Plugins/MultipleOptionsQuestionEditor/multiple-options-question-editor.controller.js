angular.module("umbraco")
    .controller("MultipleOptionsQuestionEditor",
    function ($scope) {
        $scope.answers = $scope.model.value || [];
        //$scope.inputType = $scope.model.config.multichoice !== 1 ? 'checkbox' : 'radio'; //Umbraco saves boolean values as 0 and 1
        $scope.addAnswer = function () {
           
            var answer = {
                index: $scope.answers.length,
                isCorrect: false,
                text: ''
            };

            $scope.answers.push(answer);
        };

        $scope.removeAnswer = function (index) {
            $scope.answers.splice(index, 1);
            var startIndex = (index > 0) ? index : 0;
            for (var i = startIndex; i < $scope.answers.length; i++) {
                $scope.answers[i].index--;
            }

        };

        $scope.$watch('answers', function (newAnswers) {
            var foundCorrectAnswer = false;
            newAnswers.forEach(function(answer) {
                if (answer.isCorrect) {
                    foundCorrectAnswer = true;
                }
            });

            $scope.propertyForm.$setValidity("noCorrectAnswer", foundCorrectAnswer);
            $scope.model.value = newAnswers;
        }, true);
    });