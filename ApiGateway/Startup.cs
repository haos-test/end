using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationProviderKey = "IdentityApiKey";

            // NUGET - Microsoft.AspNetCore.Authentication.JwtBearer
            services.AddAuthentication()
             .AddJwtBearer(authenticationProviderKey, x =>
             {
                 x.Authority = "http://localhost:5000"; // IDENTITY SERVER URL
                 //x.Authority = "https://localhost:44318"; // IDENTITY SERVER URL
                 //x.RequireHttpsMetadata = false;
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateAudience = false
                 };
             });
            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
            var configuration = new OcelotPipelineConfiguration
            {
                PreQueryStringBuilderMiddleware = async (ctx, next) =>
                {
                    await next.Invoke();
                    //add code here because all middleware as run at this point and the request is going to go back up the pipeline.
                },
                AuthenticationMiddleware = async (ctx, next) =>
                {
                    await next.Invoke();
                    //add code here because all middleware as run at this point and the request is going to go back up the pipeline.
                },
                PreErrorResponderMiddleware = async (ctx, next) =>
                {
                    await next.Invoke();
                    //add code here because all middleware as run at this point and the request is going to go back up the pipeline.
                },
                AuthorisationMiddleware = async (ctx, next) =>
                {
                    await next.Invoke();
                    //add code here because all middleware as run at this point and the request is going to go back up the pipeline.
                },
                PreAuthenticationMiddleware = async (ctx, next) =>
                {
                    await next.Invoke();
                    //add code here because all middleware as run at this point and the request is going to go back up the pipeline.
                },
            };

            app.UseOcelot(configuration).Wait();
        }
    }
}
