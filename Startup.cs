﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace notes
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
            services.AddSignalR();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSignalR(hubRouteBuilder => {
                hubRouteBuilder.MapHub<notes.Hubi.TextHub>("/texthub");
            });

            app.UseMvc(routes =>
            {
                 routes.MapRoute(
                    name: "home",
                    template: "notes/{**node}",                    
                    defaults: new {controller = "Home", action = "Index"} );
// routes.MapRoute(
//     name: "blog",
//     template: "Blog/{**article}",
//     defaults: new { controller = "Blog", action = "ReadArticle" });

                 routes.MapRoute(
                     name: "api",
                     template: "api/{action=Index}/{node?}",
                     defaults: new {controller = "Home"} );

                 routes.MapRoute(
                     name: "default",
                     template: "/",
                     defaults: new {controller = "Home", action = "Index"} );
            });
        }
    }
}