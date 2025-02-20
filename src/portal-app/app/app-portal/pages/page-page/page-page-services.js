﻿'use strict';
app.factory('PagePageService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonService, baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('page-page');
        var _delete = async function(pageId, postId){
            var url = this.prefixUrl + '/delete/' + pageId+'/'+postId;
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        }
        var _updateInfos = async function (pages) {

            var req = {
                method: 'POST',
                url: this.prefixUrl + '/update-infos',
                data: JSON.stringify(pages)
            };
            return await commonService.getApiResult(req);
        };
        serviceFactory.delete = _delete;
        serviceFactory.updateInfos = _updateInfos;
        return serviceFactory;

    }]);
