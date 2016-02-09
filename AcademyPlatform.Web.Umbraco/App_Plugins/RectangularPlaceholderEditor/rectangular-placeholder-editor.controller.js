angular.module("umbraco")
    .controller("RectangularPlaceholderEditor",
    function ($scope) {
        var hexColorRegex = /^(#?)[0-9a-f]{3}([0-9a-f]{3})?$/i;
        $scope.rectangle = $scope.model.value || {};

        $scope.$watch('rectangle', function (newRectangle) {

            $scope.propertyForm.$setValidity("topLeftX", !!newRectangle.topLeftX);
            $scope.propertyForm.$setValidity("topLeftY", !!newRectangle.topLeftY);
            $scope.propertyForm.$setValidity("width", !!newRectangle.width);
            $scope.propertyForm.$setValidity("height", !!newRectangle.height);
            $scope.propertyForm.$setValidity("color", !!newRectangle.color);

            if (!!newRectangle.color) {
                var validHex = hexColorRegex.test(newRectangle.color);
                if (validHex) {
                    if (newRectangle.color.indexOf('#') === -1) {
                        newRectangle.color = '#' + newRectangle.color;
                    }
                }

                $scope.propertyForm.$setValidity("colorFormat", validHex);
            }
            $scope.model.value = newRectangle;
        }, true);
    });