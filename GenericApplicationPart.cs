using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MvcMovie.Controllers;

namespace MvcMovie
{
    public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var typeInfo in EntityTypes.Types)
            {
                if (feature.Controllers.Any(x => x.Name == typeInfo.Name + "Controller")) continue;
                var controller = typeof(GenericController<>).MakeGenericType(typeInfo).GetTypeInfo();
                feature.Controllers.Add(controller);
            }
        }
    }

    public class GenericControllerNameConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            // Not a GenericController, ignore.
            if (!controller.ControllerType.IsGenericType ||
                controller.ControllerType.GetGenericTypeDefinition() != typeof(GenericController<>)) return;

            var entityType = controller.ControllerType.GenericTypeArguments[0];
            controller.ControllerName = entityType.Name;
        }
    }

    public class GenericViewLocationExpender : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            var controllerTypeInfo =
                ((ControllerActionDescriptor) context.ActionContext.ActionDescriptor).ControllerTypeInfo;
            //非泛型，跳过
            if (!controllerTypeInfo.IsGenericType) return viewLocations;
            var idx = controllerTypeInfo.Name.IndexOf("Controller", StringComparison.InvariantCulture);
            //不带Controller，直接返回移除末尾`1后的泛型名
            if (idx <= 0)
                return viewLocations.Concat(new[]
                {
                    "/Views/" + controllerTypeInfo.Name.Remove(controllerTypeInfo.Name.Length - 2, 2) +
                    "/{0}.cshtml"
                });
            //带Controller，取Controller前的名字
            var name = controllerTypeInfo.Name.Substring(0, idx);
            return viewLocations.Concat(new[] {"/Views/" + name + "/{0}.cshtml"});
        }
    }

    public static class EntityTypes
    {
        public static IReadOnlyCollection<TypeInfo> Types => new[]
        {
            typeof(Entity1).GetTypeInfo(),
            typeof(Entity2).GetTypeInfo(),
            typeof(Entity3).GetTypeInfo(),
        };
    }

    public class Entity1
    {
    }

    public class Entity2
    {
    }

    public class Entity3
    {
    }
}