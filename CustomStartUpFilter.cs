﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MvcMovie
{
        public class RequestSetOptionsMiddleware
        {
            private readonly RequestDelegate _next;
            private IOptions< AppOptions> _injectedOptions;

            public RequestSetOptionsMiddleware(
                RequestDelegate next, IOptions<AppOptions> injectedOptions)
            {
                _next = next;
                _injectedOptions = injectedOptions;
            }

            public async Task Invoke(HttpContext httpContext)
            {
                Console.WriteLine("RequestSetOptionsMiddleware.Invoke");

                var option = httpContext.Request.Query["option"];

                if (!string.IsNullOrWhiteSpace(option))
                {
                    _injectedOptions.Value.Option = WebUtility.HtmlEncode(option);
                }

                await _next(httpContext);
            }
        }
   

    public class RequestSetOptionsStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseMiddleware<RequestSetOptionsMiddleware>();
                next(builder);
            };
        }
    }

    public class AppOptions
    {
        public string Option { get; set; }
    }
}