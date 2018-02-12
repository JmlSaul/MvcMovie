using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MvcMovie
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt => opt.Conventions.Add(new GenericControllerNameConvention()))
                .ConfigureApplicationPartManager(
                    opt => opt.FeatureProviders.Add(new GenericControllerFeatureProvider()))
                .AddRazorOptions(opt => opt.ViewLocationExpanders.Add(new GenericViewLocationExpender()))
                ;
            services.AddTransient<IStartupFilter, RequestSetOptionsStartupFilter>();
//            var x = ((Expression<Func<object>>) (() => Expression.New(typeof(Movie)))).Compile();
//            var ccc = ((NewExpression) x()).Constructor.Invoke(null);
            // services.Configure<RazorViewEngineOptions>(options =>
            //             {
            //                 options.AreaViewLocationFormats.Clear();
            //                 options.AreaViewLocationFormats.Add("/Categories/{2}/Views/{1}/{0}.cshtml");
            //                 options.AreaViewLocationFormats.Add("/Categories/{2}/Views/Shared/{0}.cshtml");
            //                 options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            //             });

            services.AddDbContext<MvcMovieContext>(options =>
                options.UseSqlite("Data Source=MvcMovie.db")
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}