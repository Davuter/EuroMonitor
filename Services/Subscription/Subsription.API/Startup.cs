using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Subscription.Application.Interfaces;
using Subscription.Application.Middlewares;
using Subscription.Application.Subscriptions.Command.Add;
using Subscription.Application.Subscriptions.Query;
using Subscription.Storage;
using Subsription.API.Filter;
using Subsription.API.Middlewares;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace Subsription.API
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
            services.AddControllers();

            #region DbContext
            string cnnString = Configuration.GetConnectionString("DBConnection");

            services.AddDbContext<ISubscriptionDbContext, SubscriptionDbContext>(options =>
                options.UseSqlServer(cnnString), ServiceLifetime.Scoped);
            #endregion
            #region Authentication
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetValue<string>("IdentityUrl");

                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.ValidateAudience = false;
             

            });

            //// accepts any access token issued by identity server
            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", options =>
            //    {
            //        options.Authority = Configuration.GetValue<string>("IdentityUrl");

            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateAudience = false
            //        };
            //    });

            //// adds an authorization policy to make sure the token is for scope 'api1'
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ApiScope", policy =>
            //    {
            //        policy.RequireAuthenticatedUser();
            //        policy.RequireClaim("scope", "subscriptions");
            //    });
            //});


            #endregion

            services
           .AddMvc(options =>
           {

               options.Filters.Add(typeof(CustomExceptionFilterAttribute));
           });

            // Add MediatR
            services.AddMediatR(typeof(SubscriptionQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(SubscriptionAddHandler).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Subscription HTTP API",
                    Version = "v1",
                    Description = "Subscription Works",
                });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
                            TokenUrl = new Uri($"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                             {
                                { "subscriptions", "Subscription API" }
                             }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            //var container = new ContainerBuilder();
            //container.Populate(services);
            //return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRequestMiddleware();

            app.UseRouting();


            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Subscription API");
                c.RoutePrefix = "help";
                c.OAuthClientId("subscriptionswaggerui");
                c.OAuthAppName("Subscription Swagger UI");
 
            });
            ConfigureAuth(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                 
            });
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration.GetValue<bool>("UseTest"))
            {
                app.UseMiddleware<ByPassAuthMiddleware>();
            }

            app.UseAuthentication();
            app.UseAuthorization();
        }


    }
}
