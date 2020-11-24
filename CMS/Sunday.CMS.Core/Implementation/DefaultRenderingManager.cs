﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Contents;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IRenderingManager))]
    public class DefaultRenderingManager : IRenderingManager
    {
        private readonly IRenderingService _renderingService;
        private readonly IEntityAccessService _entityAccessService;

        public DefaultRenderingManager(IRenderingService renderingService, IEntityAccessService entityAccessService)
        {
            _renderingService = renderingService;
            _entityAccessService = entityAccessService;
        }

        public Task<ListApiResponse<RenderingJsonResult>> Search(RenderingQuery query)
            => _renderingService.Search(query).MapResultTo(rs => ListApiResponse<RenderingJsonResult>.From(rs, ToJsonResult));

        public async Task<RenderingJsonResult> GetById(Guid id)
        {
            var renderingOpt = await _renderingService.GetRenderingById(id).MapResultTo(rs => rs.Map(ToJsonResult));
            if (renderingOpt.IsNone) return BaseApiResponse.ErrorResult<RenderingJsonResult>("Rendering not found");
            var template = renderingOpt.Get();
            var access = await _entityAccessService.GetByEntity(id, nameof(Rendering));
            access.IfSome(acc => template.Access = acc);
            return template;
        }

        public async Task<BaseApiResponse> Create(RenderingJsonResult rendering)
        {
            rendering.Id = Guid.Empty;
            //TODO: select template
            rendering.DatasourceTemplate = Guid.Empty.ToString();
            var model = ToModel(rendering);
            await _renderingService.Save(model);
            await _entityAccessService.Save(GetEntityAccess(rendering.Access!, model.Id));
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> Update(RenderingJsonResult rendering)
        {
            rendering.DatasourceTemplate = Guid.Empty.ToString();
            await _renderingService.Save(ToModel(rendering));
            await _entityAccessService.Save(GetEntityAccess(rendering.Access!, rendering.Id!.Value));
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> Delete(Guid id)
        {
            await _renderingService.Delete(id);
            return BaseApiResponse.SuccessResult;
        }

        EntityAccess GetEntityAccess(EntityAccess entityAccess, Guid renderingId)
        {
            var access = entityAccess;
            access.EntityType = nameof(Rendering);
            access.EntityId = renderingId;
            return access;
        }

        RenderingJsonResult ToJsonResult(Rendering model)
        {
            var jsonResult = model.MapTo<RenderingJsonResult>();
            jsonResult.Action = model.Properties.Get("Action").IfNone(string.Empty);
            jsonResult.Controller = model.Properties.Get("Controller").IfNone(string.Empty);
            return jsonResult;
        }

        Rendering ToModel(RenderingJsonResult jsonResult)
        {
            var model = jsonResult.MapTo<Rendering>();
            model.Properties = new Dictionary<string, string>()
            {
                {"Action", jsonResult.Action},
                {"Controller", jsonResult.Controller},
            };
            model.RenderingType = "ControllerRendering";
            return model;
        }
    }
}
