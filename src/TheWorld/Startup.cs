﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public Startup(IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(configuration =>
            {
#if !DEBUG
                //configuration.Filters.Add(new RequireHttpsAttribute());
#endif
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
                        
            services.AddLogging();
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<WorldContext>();
                                            
            services.AddIdentity<WorldUser, IdentityRole>(configuration =>
                {
                    configuration.SignIn.RequireConfirmedPhoneNumber = false;
                    configuration.SignIn.RequireConfirmedEmail = false;
                    configuration.User.RequireUniqueEmail = true;
                    configuration.Password.RequiredLength = 5;
                    configuration.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                    configuration.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                    {
                         OnRedirectToLogin = ctx =>
                         {
                             if (ctx.Request.Path.StartsWithSegments("/api") && 
                                 ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                             {
                                 ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                             }
                             else
                             {
                                 ctx.Response.Redirect(ctx.RedirectUri);
                             }
                             return Task.FromResult(0);
                         }
                    };
                })
                .AddEntityFrameworkStores<WorldContext>();

            services.AddScoped<CoordService>();
            services.AddTransient<WorldContextSeedData>();
            services.AddScoped<IWorldRepository, WorldRepository>();
            services.AddScoped<IMailService, DebugMailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, WorldContextSeedData seeder, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment)
        {
            // Utiliser pour le déploiement sur IIS
            //app.UseIISPlatformHandler();

            if (hostingEnvironment.IsDevelopment())
            {
                loggerFactory.AddDebug(LogLevel.Information);
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error);
                //app.UseExceptionHandler("/App/Error");
            }

            app.UseStaticFiles();
            app.UseIdentity();

            Mapper.Initialize(configuration =>
            {
                configuration.CreateMap<Trip, TripViewModel>().ReverseMap();
                configuration.CreateMap<Stop, StopViewModel>().ReverseMap();
            });

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
                );
            });

            await seeder.EnsureSeedDataAsync();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}