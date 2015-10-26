angular.module("umbraco")
    .controller("SingleChoiceQuestionEditor",
    function ($scope) {
        $scope.answers = $scope.model.value || [];

        $scope.addAnswer = function () {
        
            var answer = {
                id: $scope.answers.length,
                isCorrect: false,
                text: ''
            };

            $scope.answers.push(answer);
        };

        $scope.removeAnswer = function (index) {
            $scope.answers.splice(index, 1);
        };

        $scope.$watch('answers', function (newAnswers) {
            $scope.model.value = newAnswers;
        }, true);
    });