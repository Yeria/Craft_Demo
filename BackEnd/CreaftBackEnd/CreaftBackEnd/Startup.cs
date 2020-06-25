using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraftBackEnd.Services;
using CraftBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using CraftBackEnd.Services.Entity;
using Microsoft.EntityFrameworkCore;
using CraftBackEnd.Common.Configs;
using CraftBackEnd.Services.Core;
using Microsoft.AspNetCore.Server.IISIntegration;
using CraftBackEnd.Extensions;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace CraftBackEnd
{
    public class Startup
    {
        readonly string AllowedOrigins = "AllowAll";
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger) {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddCors(options => {
                options.AddPolicy(name: AllowedOrigins,
                    builder => {
                        builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                        builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                        builder.WithOrigins("https://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                        builder.WithOrigins("https://localhost:8443").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                    }
                );
            });
            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddControllers();

            // loading configs from appsettings.json
            var authOptionsSection = Configuration.GetSection("AuthOptions");
            services.Configure<ErrorMessages>(Configuration.GetSection("ErrorMessages"));
            services.Configure<TierCountLimit>(Configuration.GetSection("TierCountLimit"));
            services.Configure<AuthOptions>(authOptionsSection);

            services.AddScoped<ILoggerFacade, LoggerFacade>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserTierService, UserTierService>();
            services.AddScoped<ICalculatorService, CalculatorService>();
            services.AddScoped<IIAMService, IAMService>();
            services.AddScoped<IValidationService, ValidationService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // setting up API versioning
            services.AddApiVersioning(o => {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(new DateTime(2020, 6, 20), 1, 0);
            });

            // setting up DB configs
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    builder => {
                        builder.MigrationsAssembly("CraftBackEnd.Migrations");
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    }));

            services.AddScoped<IDatabaseContext, DatabaseContext>();

            // configure jwt authentication
            var authOptions = authOptionsSection.Get<AuthOptions>();
            var key = Encoding.ASCII.GetBytes(authOptions.TokenKey);
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // setting up swagger gen
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebServices API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddNLog();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            if (!env.IsProduction()) {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebServices API");
                });

                //loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Information);
            }

            app.ConfigureExceptionHandler(_logger);
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(AllowedOrigins);
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
