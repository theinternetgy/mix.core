﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixSystem;
using Mix.Common.Helper;
using Mix.Domain.Core.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class UpdateViewModel
         : ViewModelBase<MixCmsContext, MixPost, UpdateViewModel>
    {

        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonIgnore]
        [JsonProperty("extraFields")]
        public string ExtraFields { get; set; } = "[]";

        [JsonIgnore]
        [JsonProperty("extraProperties")]
        public string ExtraProperties { get; set; } = "[]";

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [Required]
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("seoName")]
        public string SeoName { get; set; }

        [JsonProperty("seoTitle")]
        public string SeoTitle { get; set; }

        [JsonProperty("seoDescription")]
        public string SeoDescription { get; set; }

        [JsonProperty("seoKeywords")]
        public string SeoKeywords { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("views")]
        public int? Views { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("publishedDateTime")]
        public DateTime? PublishedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; } = "[]";

        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain => MixService.GetConfig<string>("Domain");

        [JsonProperty("categories")]
        public List<MixPagePosts.ReadViewModel> Pages { get; set; }

        [JsonProperty("modules")]
        public List<MixModulePosts.ReadViewModel> Modules { get; set; } // Parent to Modules

        [JsonProperty("mediaNavs")]
        public List<MixPostMedias.ReadViewModel> MediaNavs { get; set; }

        [JsonProperty("moduleNavs")]
        public List<MixPostModules.ReadViewModel> ModuleNavs { get; set; }

        [JsonProperty("postNavs")]
        public List<MixPostPosts.ReadViewModel> PostNavs { get; set; }

        [JsonProperty("listTag")]
        public JArray ListTag { get; set; } = new JArray();

        [JsonProperty("imageFileStream")]
        public FileStreamViewModel ImageFileStream { get; set; }

        [JsonProperty("thumbnailFileStream")]
        public FileStreamViewModel ThumbnailFileStream { get; set; }
        [JsonProperty("attributes")]
        public MixAttributeSets.UpdateViewModel Attributes { get; set; }
        [JsonProperty("attributeData")]
        public MixRelatedAttributeDatas.UpdateViewModel AttributeData { get; set; }

        #region Template

        [JsonProperty("view")]
        public MixTemplates.UpdateViewModel View { get; set; }

        [JsonProperty("templates")]
        public List<MixTemplates.UpdateViewModel> Templates { get; set; }// Post Templates

        [JsonIgnore]
        public int ActivedTheme
        {
            get
            {
                return MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, Specificulture);
            }
        }

        [JsonIgnore]
        public string TemplateFolderType
        {
            get
            {
                return MixEnums.EnumTemplateFolder.Posts.ToString();
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[]
                {
                    MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, Specificulture)
                    , TemplateFolderType
                }
            );
            }
        }

        #endregion Template

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Image
                });
                }
                else
                {
                    return Image;
                }
            }
        }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl
        {
            get
            {
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1 && Thumbnail[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Thumbnail
                });
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        [JsonProperty("properties")]
        public List<ExtraProperty> Properties { get; set; }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("urlAliases")]
        public List<MixUrlAliases.UpdateViewModel> UrlAliases { get; set; }

        [JsonProperty("columns")]
        public List<ModuleFieldViewModel> Columns { get; set; }
        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                ExtraFields = MixService.GetConfig<string>("DefaultPostAttr");
            }
            Cultures = LoadCultures(Specificulture, _context, _transaction);
            UrlAliases = GetAliases(_context, _transaction);
            if (!string.IsNullOrEmpty(this.Tags))
            {
                ListTag = JArray.Parse(this.Tags);
            }

            // Load Attributes
            LoadAttributes(_context, _transaction);

            //// Load Properties
            //LoadExtraProperties();

            //Get Templates
            LoadTemplates(_context, _transaction);

            // Load Parent Pages
            LoadParentPage(_context, _transaction);

            // Load Parent Modules
            LoadParentModules(_context, _transaction);

            // Medias
            LoadMedias(_context, _transaction);

            //// Sub Modules
            //LoadSubModules(_context, _transaction);

            // Related Posts
            LoadRelatedPost(_context, _transaction);
        }

        public override MixPost ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(c => c.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            LastModified = DateTime.UtcNow;
            PublishedDateTime = PublishedDateTime?.ToUniversalTime();

            //  Parsing Extra Fields to json string
            var arrField = Columns != null ? JArray.Parse(
                Newtonsoft.Json.JsonConvert.SerializeObject(Columns.OrderBy(c => c.Priority).Where(
                    c => !string.IsNullOrEmpty(c.Name)))) : new JArray();
            ExtraFields = arrField.ToString(Newtonsoft.Json.Formatting.None);

            // Parsing Extra Properties value
            if (Properties != null && Properties.Count > 0)
            {
                JArray arrProperties = new JArray();
                foreach (var p in Properties.Where(p => !string.IsNullOrEmpty(p.Value) && !string.IsNullOrEmpty(p.Name)))
                {
                    arrProperties.Add(JObject.FromObject(p));
                }
                ExtraProperties = arrProperties.ToString(Formatting.None)?.Trim();
            }

            Template = View != null ? string.Format(@"{0}/{1}{2}", View.FolderType, View.FileName, View.Extension) : Template;
            if (ThumbnailFileStream != null)
            {
                string folder = CommonHelper.GetFullPath(new string[]
                {
                    MixConstants.Folder.UploadFolder, "Posts", DateTime.UtcNow.ToString("dd-MM-yyyy")
                });
                string filename = CommonHelper.GetRandomName(ThumbnailFileStream.Name);
                bool saveThumbnail = CommonHelper.SaveFileBase64(folder, filename, ThumbnailFileStream.Base64);
                if (saveThumbnail)
                {
                    CommonHelper.RemoveFile(Thumbnail);
                    Thumbnail = CommonHelper.GetFullPath(new string[] { folder, filename });
                }
            }
            if (ImageFileStream != null)
            {
                string folder = CommonHelper.GetFullPath(new string[]
                {
                    MixConstants.Folder.UploadFolder, "Posts", DateTime.UtcNow.ToString("dd-MM-yyyy")
                });
                string filename = CommonHelper.GetRandomName(ImageFileStream.Name);
                bool saveImage = CommonHelper.SaveFileBase64(folder, filename, ImageFileStream.Base64);
                if (saveImage)
                {
                    CommonHelper.RemoveFile(Image);
                    Image = CommonHelper.GetFullPath(new string[] { folder, filename });
                }
            }

            if (!string.IsNullOrEmpty(Image) && Image[0] == '/') { Image = Image.Substring(1); }
            if (!string.IsNullOrEmpty(Thumbnail) && Thumbnail[0] == '/') { Thumbnail = Thumbnail.Substring(1); }
            Tags = ListTag.ToString(Newtonsoft.Json.Formatting.None);
            GenerateSEO();

            return base.ParseModel(_context, _transaction);
        }

        #region Async Methods
        #region Save Sub Models Async
        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(
            MixPost parent
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                // Save Template
                var saveTemplate = await View.SaveModelAsync(true, _context, _transaction);
                result.IsSucceed = result.IsSucceed && saveTemplate.IsSucceed;
                ViewModelHelper.HandleResult(saveTemplate, ref result);

                if (result.IsSucceed)
                {
                    // Save Alias
                    result = await SaveUrlAliasAsync(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed)
                {
                    // Save Medias
                    result = await SaveMediasAsync(parent.Id, _context, _transaction);
                }
                //if (result.IsSucceed)
                //{
                //    // Save Sub Modules
                //    result = await SaveSubModulesAsync(parent.Id, _context, _transaction);
                //}

                if (result.IsSucceed)
                {
                    // Save Attributes
                    result = await SaveAttributeAsync(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed)
                {
                    // Save related posts
                    result = await SaveRelatedPostAsync(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed)
                {
                    // Save Parent Category
                    result = await SaveParentPagesAsync(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed)
                {
                    // Save Parent Modules
                    result = await SaveParentModulesAsync(parent.Id, _context, _transaction);
                }

                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
            }
        }

        private async Task<RepositoryResponse<bool>> SaveAttributeAsync(int id, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            AttributeData.ParentId = id.ToString();
            AttributeData.ParentType = (int)MixEnums.MixAttributeSetDataType.Post;
            var saveData = await AttributeData.Data.SaveModelAsync(true, context, transaction);
            ViewModelHelper.HandleResult(saveData, ref result);
            if (result.IsSucceed)
            {
                AttributeData.Id = saveData.Data.Id;
                var saveRelated = await AttributeData.SaveModelAsync(true, context, transaction);
                ViewModelHelper.HandleResult(saveRelated    , ref result);
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveParentModulesAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Modules)
            {
                item.PostId = id;
                item.Status = MixEnums.MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = await item.RemoveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveParentPagesAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Pages)
            {
                item.PostId = id;
                item.Status = MixEnums.MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = await item.RemoveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveRelatedPostAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navPost in PostNavs)
            {
                navPost.SourceId = id;
                navPost.Status = MixEnums.MixContentStatus.Published;
                navPost.Specificulture = Specificulture;
                if (navPost.IsActived)
                {
                    var saveResult = await navPost.SaveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = await navPost.RemoveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        //private async Task<RepositoryResponse<bool>> SaveSubModulesAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        //{
        //    var result = new RepositoryResponse<bool>() { IsSucceed = true };
        //    foreach (var navModule in ModuleNavs)
        //    {
        //        navModule.PostId = id;
        //        navModule.Specificulture = Specificulture;
        //        navModule.Status = MixEnums.MixContentStatus.Published;
        //        if (navModule.IsActived)
        //        {
        //            var saveResult = await navModule.SaveModelAsync(false, _context, _transaction);
        //            ViewModelHelper.HandleResult(saveResult, ref result);
        //        }
        //        else
        //        {
        //            var saveResult = await navModule.RemoveModelAsync(false, _context, _transaction);
        //            ViewModelHelper.HandleResult(saveResult, ref result);
        //        }
        //    }
        //    return result;
        //}

        private async Task<RepositoryResponse<bool>> SaveMediasAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navMedia in MediaNavs)
            {
                navMedia.PostId = id;
                navMedia.Specificulture = Specificulture;

                if (navMedia.IsActived)
                {
                    var saveResult = await navMedia.SaveModelAsync(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    var saveResult = await navMedia.RemoveModelAsync(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveUrlAliasAsync(int parentId, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in UrlAliases)
            {
                item.SourceId = parentId.ToString();
                item.Type = MixEnums.UrlAliasType.Post;
                item.Specificulture = Specificulture;
                var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
                if (!result.IsSucceed)
                {
                    break;
                }
            }
            return result;
        }

        #endregion

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>()
            {
                IsSucceed = true
            };

            if (result.IsSucceed)
            {
                var navCate = await _context.MixPagePost.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navCate)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navModule = await _context.MixModulePost.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navMedia = await _context.MixPostMedia.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navMedia)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }
            if (result.IsSucceed)
            {
                var navModule = await _context.MixPostModule.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navRelated = await _context.MixPostMedia.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navRelated)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navs = await _context.MixUrlAlias.Where(n => n.SourceId == Id.ToString() && n.Type == (int)MixEnums.UrlAliasType.Post && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navs)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            result.IsSucceed = (await _context.SaveChangesAsync() > 0);
            return result;
        }

        public override Task<RepositoryResponse<List<UpdateViewModel>>> CloneAsync(MixPost model, List<SupportedCulture> cloneCultures, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            return base.CloneAsync(model, cloneCultures, _context, _transaction);
        }
        #endregion Async Methods

        #region Sync Methods

        #region Save Sub Models
        public override RepositoryResponse<bool> SaveSubModels(
            MixPost parent
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                // Save Template
                var saveTemplate = View.SaveModel(true, _context, _transaction);
                result.IsSucceed = result.IsSucceed && saveTemplate.IsSucceed;
                ViewModelHelper.HandleResult(saveTemplate, ref result);

                if (result.IsSucceed)
                {
                    // Save Alias
                    result = SaveUrlAlias(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed)
                {
                    // Save Medias
                    result = SaveMedias(parent.Id, _context, _transaction);
                }
                //if (result.IsSucceed)
                //{
                //    // Save Sub Modules
                //    result = SaveSubModules(parent.Id, _context, _transaction);
                //}

                if (result.IsSucceed)
                {
                    // Save Attributes
                    result = SaveAttribute(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed)
                {
                    // Save related posts
                    result = SaveRelatedPost(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed)
                {
                    // Save Parent Category
                    result = SaveParentPages(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed)
                {
                    // Save Parent Modules
                    result = SaveParentModules(parent.Id, _context, _transaction);
                }

                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
            }
        }

        private RepositoryResponse<bool> SaveAttribute(int id, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            AttributeData.ParentId = id.ToString();
            AttributeData.ParentType = (int)MixEnums.MixAttributeSetDataType.Post;
            var saveData = AttributeData.SaveModel(true, context, transaction);
            ViewModelHelper.HandleResult(saveData, ref result);
            return result;
        }

        private RepositoryResponse<bool> SaveParentModules(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Modules)
            {
                item.PostId = id;
                item.Description = Title;
                item.Image = ThumbnailUrl;
                item.Status = MixEnums.MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = item.SaveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = item.RemoveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveParentPages(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Pages)
            {
                item.PostId = id;
                item.Description = Title;
                item.Image = ThumbnailUrl;
                item.Status = MixEnums.MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = item.SaveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = item.RemoveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveRelatedPost(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navPost in PostNavs)
            {
                navPost.SourceId = id;
                navPost.Status = MixEnums.MixContentStatus.Published;
                navPost.Specificulture = Specificulture;
                if (navPost.IsActived)
                {
                    var saveResult = navPost.SaveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = navPost.RemoveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveMedias(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navMedia in MediaNavs)
            {
                navMedia.PostId = id;
                navMedia.Specificulture = Specificulture;

                if (navMedia.IsActived)
                {
                    var saveResult = navMedia.SaveModel(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    var saveResult = navMedia.RemoveModel(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveUrlAlias(int parentId, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in UrlAliases)
            {
                item.SourceId = parentId.ToString();
                item.Type = MixEnums.UrlAliasType.Post;
                item.Specificulture = Specificulture;
                var saveResult = item.SaveModel(false, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
                if (!result.IsSucceed)
                {
                    break;
                }
            }
            return result;
        }

        #endregion

        public override RepositoryResponse<bool> RemoveRelatedModels(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>()
            {
                IsSucceed = true
            };

            if (result.IsSucceed)
            {
                var navCate = _context.MixPagePost.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navCate)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navModule = _context.MixModulePost.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navMedia = _context.MixPostMedia.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navMedia)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }
            if (result.IsSucceed)
            {
                var navModule = _context.MixPostModule.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navRelated = _context.MixPostMedia.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navRelated)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navs = _context.MixUrlAlias.Where(n => n.SourceId == Id.ToString() && n.Type == (int)MixEnums.UrlAliasType.Post && n.Specificulture == Specificulture).ToList();
                foreach (var item in navs)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            result.IsSucceed = (_context.SaveChanges() > 0);
            return result;
        }

        #endregion  Methods

        #endregion Overrides

        #region Expands

        private void LoadRelatedPost(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            PostNavs = GetRelated(_context, _transaction);            
        }

        private void LoadMedias(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getPostMedia = MixPostMedias.ReadViewModel.Repository.GetModelListBy(n => n.PostId == Id && n.Specificulture == Specificulture, _context, _transaction);
            if (getPostMedia.IsSucceed)
            {
                MediaNavs = getPostMedia.Data.OrderBy(p => p.Priority).ToList();
                MediaNavs.ForEach(n => n.IsActived = true);
            }
        }

        private void LoadParentModules(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getModulePost = MixModulePosts.ReadViewModel.GetModulePostNavAsync(Id, Specificulture, _context, _transaction);
            if (getModulePost.IsSucceed)
            {
                foreach (var item in getModulePost.Data)
                {
                    item.Description = item.Module.Title;
                    item.Image = item.Module.ImageUrl;
                }
                this.Modules = getModulePost.Data;
                this.Modules.ForEach(c =>
                {
                    c.IsActived = MixModulePosts.ReadViewModel.Repository.CheckIsExists(n => n.ModuleId == c.ModuleId && n.PostId == Id, _context, _transaction);
                });
            }
            var otherModules = MixModules.ReadListItemViewModel.Repository.GetModelListBy(
                m => (m.Type == (int)MixEnums.MixModuleType.Content || m.Type == (int)MixEnums.MixModuleType.ListPost)
                && m.Specificulture == Specificulture
                && !Modules.Any(n => n.ModuleId == m.Id && n.Specificulture == m.Specificulture)
                , "CreatedDateTime", 1, null, 0, _context, _transaction);
            foreach (var item in otherModules.Data.Items)
            {
                Modules.Add(new MixModulePosts.ReadViewModel()
                {
                    ModuleId = item.Id,
                    Image = item.Image,
                    PostId = Id,
                    Description = item.Title
                });
            }

        }

        private void LoadParentPage(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getPagePost = MixPagePosts.ReadViewModel.GetPagePostNavAsync(Id, Specificulture, _context, _transaction);
            if (getPagePost.IsSucceed)
            {
                this.Pages = getPagePost.Data;
                this.Pages.ForEach(c =>
                {
                    c.Description = c.Page.Title;
                    c.IsActived = MixPagePosts.ReadViewModel.Repository.CheckIsExists(n => n.PageId == c.PageId && n.PostId == Id, _context, _transaction);
                });
            }
        }

        private void LoadTemplates(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            this.Templates = this.Templates ?? MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Id == ActivedTheme && t.FolderType == this.TemplateFolderType).Data;
            View = MixTemplates.UpdateViewModel.GetTemplateByPath(Template, Specificulture, MixEnums.EnumTemplateFolder.Posts, _context, _transaction);

            this.Template = CommonHelper.GetFullPath(new string[]
               {
                    this.View?.FileFolder
                    , this.View?.FileName
               });           
        }

        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getAttrs = MixAttributeSets.UpdateViewModel.Repository.GetSingleModel(m => m.Name == "post", _context, _transaction);
            if (getAttrs.IsSucceed)
            {
                Attributes = getAttrs.Data;
                AttributeData = MixRelatedAttributeDatas.UpdateViewModel.Repository.GetFirstModel(
                    a => a.ParentId == Id.ToString() && a.Specificulture == Specificulture && a.AttributeSetId == Attributes.Id
                        , _context, _transaction).Data;
                if (AttributeData == null)
                {
                    AttributeData = new MixRelatedAttributeDatas.UpdateViewModel(
                        new MixRelatedAttributeData()
                        {
                            Specificulture = Specificulture,
                            ParentType = (int)MixEnums.MixAttributeSetDataType.Post,
                            ParentId = Id.ToString(),
                            AttributeSetId = Attributes.Id,
                            AttributeSetName = Attributes.Name
                        }
                        )
                    {
                        Data = new MixAttributeSetDatas.UpdateViewModel(
                    new MixAttributeSetData()
                    {
                        Specificulture = Specificulture,
                        AttributeSetId = Attributes.Id,
                        AttributeSetName = Attributes.Name
                    }
                    )
                    };
                }
                foreach (var field in Attributes.Fields.OrderBy(f => f.Priority))
                {
                    var val = AttributeData.Data.Values.FirstOrDefault(v => v.AttributeFieldId == field.Id);
                    if (val == null)
                    {
                        val = new MixAttributeSetValues.UpdateViewModel(
                            new MixAttributeSetValue() { AttributeFieldId = field.Id }
                            , _context, _transaction)
                        {
                            Field = field,
                            AttributeFieldName = field.Name,
                            Priority = field.Priority,
                            StringValue = field.DefaultValue
                        };
                        AttributeData.Data.Values.Add(val);
                    }
                    val.Priority = field.Priority;
                    val.Field = field;
                }
            }
        }

        //private void LoadExtraProperties()
        //{
        //    // Parsing Extra Properties fields
        //    Columns = new List<ModuleFieldViewModel>();
        //    JArray arrField = !string.IsNullOrEmpty(ExtraFields) ? JArray.Parse(ExtraFields) : new JArray();
        //    foreach (var field in arrField)
        //    {
        //        ModuleFieldViewModel thisField = new ModuleFieldViewModel()
        //        {
        //            Name = CommonHelper.ParseJsonPropertyName(field["name"].ToString()),
        //            Title = field["title"]?.ToString(),
        //            DefaultValue = field["defaultValue"]?.ToString(),
        //            Options = field["options"] != null ? field["options"].Value<JArray>() : new JArray(),
        //            Priority = field["priority"] != null ? field["priority"].Value<int>() : 0,
        //            DataType = (MixDataType)(int)field["dataType"],
        //            Width = field["width"] != null ? field["width"].Value<int>() : 3,
        //            IsUnique = field["isUnique"] != null ? field["isUnique"].Value<bool>() : true,
        //            IsRequired = field["isRequired"] != null ? field["isRequired"].Value<bool>() : true,
        //            IsDisplay = field["isDisplay"] != null ? field["isDisplay"].Value<bool>() : true,
        //            IsSelect = field["isSelect"] != null ? field["isSelect"].Value<bool>() : false,
        //            IsGroupBy = field["isGroupBy"] != null ? field["isGroupBy"].Value<bool>() : false,
        //        };
        //        Columns.Add(thisField);
        //    }

        //    // Parsing Extra Properties value
        //    Properties = new List<ExtraProperty>();

        //    if (!string.IsNullOrEmpty(ExtraProperties))
        //    {
        //        JArray arr = JArray.Parse(ExtraProperties);
        //        foreach (JToken item in arr)
        //        {
        //            var property = item.ToObject<ExtraProperty>();
        //            Properties.Add(property);
        //        }
        //    }
        //}

        List<SupportedCulture> LoadCultures(string initCulture = null, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getCultures = SystemCultureViewModel.Repository.GetModelList(_context, _transaction);
            var result = new List<SupportedCulture>();
            if (getCultures.IsSucceed)
            {
                foreach (var culture in getCultures.Data)
                {
                    result.Add(
                        new SupportedCulture()
                        {
                            Icon = culture.Icon,
                            Specificulture = culture.Specificulture,
                            Alias = culture.Alias,
                            FullName = culture.FullName,
                            Description = culture.FullName,
                            Id = culture.Id,
                            Lcid = culture.Lcid,
                            IsSupported = culture.Specificulture == initCulture || _context.MixPost.Any(p => p.Id == Id && p.Specificulture == culture.Specificulture)
                        });

                }
            }
            return result;
        }

        private void GenerateSEO()
        {
            if (string.IsNullOrEmpty(this.SeoName))
            {
                this.SeoName = SeoHelper.GetSEOString(this.Title);
            }
            int i = 1;
            string name = SeoName;
            while (UpdateViewModel.Repository.CheckIsExists(a => a.SeoName == name && a.Specificulture == Specificulture && a.Id != Id))
            {
                name = SeoName + "_" + i;
                i++;
            }
            SeoName = name;

            if (string.IsNullOrEmpty(this.SeoTitle))
            {
                this.SeoTitle = SeoHelper.GetSEOString(this.Title);
            }

            if (string.IsNullOrEmpty(this.SeoDescription))
            {
                this.SeoDescription = SeoHelper.GetSEOString(this.Title);
            }

            if (string.IsNullOrEmpty(this.SeoKeywords))
            {
                this.SeoKeywords = SeoHelper.GetSEOString(this.Title);
            }
        }

        public List<MixPostPosts.ReadViewModel> GetRelated(MixCmsContext context, IDbContextTransaction transaction)
        {
            var navs = MixPostPosts.ReadViewModel.Repository.GetModelListBy(n => n.SourceId == Id && n.Specificulture == Specificulture, context, transaction).Data;
            navs.ForEach(n => n.IsActived = true);
            return navs.OrderBy(p => p.Priority).ToList();
        }
        public List<MixUrlAliases.UpdateViewModel> GetAliases(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = MixUrlAliases.UpdateViewModel.Repository.GetModelListBy(p => p.Specificulture == Specificulture
                        && p.SourceId == Id.ToString() && p.Type == (int)MixEnums.UrlAliasType.Post, context, transaction);
            if (result.IsSucceed)
            {
                return result.Data;
            }
            else
            {
                return new List<MixUrlAliases.UpdateViewModel>();
            }
        }
        #endregion Expands
    }
}
