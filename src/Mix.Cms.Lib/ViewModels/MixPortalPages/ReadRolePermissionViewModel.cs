﻿using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixPortalPages
{
    public class ReadRolePermissionViewModel
       : ViewModelBase<MixCmsContext, MixPortalPage, ReadRolePermissionViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("textKeyword")]
        public string TextKeyword { get; set; }

        [JsonProperty("textDefault")]
        public string TextDefault { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Descriotion { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("childPages")]
        public List<MixPortalPagePortalPages.ReadViewModel> ChildPages { get; set; } = new List<MixPortalPagePortalPages.ReadViewModel>();

        [JsonProperty("navPermission")]
        public MixPortalPageRoles.ReadViewModel NavPermission { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadRolePermissionViewModel() : base()
        {
        }

        public ReadRolePermissionViewModel(MixPortalPage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getChilds = MixPortalPagePortalPages.ReadViewModel.Repository.GetModelListBy(n => n.ParentId == Id, _context, _transaction);
            if (getChilds.IsSucceed)
            {
                ChildPages = getChilds.Data.OrderBy(c => c.Priority).ToList();
            }
        }

        #endregion Overrides
    }
}
