using Bandit.ACS.Daemon.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Bandit.ACS.Daemon.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddCommandHandling(this IServiceCollection services, Action<ICommandHandlingBuilder> configure)
        {
            var builder = new CommandHandlingBuilder(services);
            configure(builder);
            builder.RegisterHandlers();
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JWTConfiguration config)
        {
            services.
                AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config.Issuer,
                        ValidAudience = config.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key)),
                    };
                });

            return services;
        }

        public static IServiceCollection AddSwaggerService(this IServiceCollection services, APIConfiguration config)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerSecurityFilter>();

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = config?.Title,
                        Description = config?.Description,
                        Version = "v1"
                    });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    },

                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,

                    Description = "JWT Authorization Header - Just insert your token below.",
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection AddCorsHandling(this IServiceCollection services) => services.AddCors(o =>
        {
            o.AddPolicy("ALLOW_ANY", b =>
            {
                b.AllowAnyMethod().
                AllowAnyHeader().
                AllowCredentials().
                SetIsOriginAllowed(hostName => true);
            });
        });


        public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app, Action<ExceptionHandlerMiddlewareOptions> configureOptions)
        {
            var options = new ExceptionHandlerMiddlewareOptions();
            configureOptions(options);
            return app.UseMiddleware<ExceptionHandlerMiddleware>(options);
        }


    }
}
