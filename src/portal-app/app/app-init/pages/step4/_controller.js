﻿'use strict';
app.controller('Step4Controller', ['$scope', '$rootScope', 
    'CommonService', 'Step4Services',
    function ($scope, $rootScope, commonService, service) {
        var rand = Math.random();
        $scope.data = [];
        $scope.init = async function () {
            var getData = await commonService.loadJArrayData('languages.json');
            if(getData.isSucceed){
                $scope.data = getData.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }else {
                if (getData) {
                    $rootScope.showErrors(getData.errors);
                }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
           
        };
        $scope.loadProgress = async function (percent) {
            var elem = document.getElementsByClassName("progress-bar")[0]; 
            elem.style.width = percent + '%'; 
        };
        $scope.submit = async function () {
            $rootScope.isBusy = true;            
            var result = await service.submit($scope.data);
            if (result.isSucceed) {
                $rootScope.isBusy = false;
                window.top.location  = '/';
            }
            else {
                if (result) { $rootScope.showErrors(result.errors); }
                $rootScope.isBusy = false;                
            }
        }
    }]);