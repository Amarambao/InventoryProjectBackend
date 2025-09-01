using BusinessLayer.Interfaces.Bogus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace API.Settings
{
    public class DI
    {
        public static async Task Add(WebApplicationBuilder builder)
        {
            BusinessLayer.Settings.DI.Add(builder);
            AddSettings(builder.Services, builder.Configuration);
            await AppBuild(builder);
        }

        private static void AddSettings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddCookiePolicy(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                        ValidateLifetime = true,
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JSON Web Token based security",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        private static async Task AppBuild(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            var corsSection = app.Configuration.GetSection("CORS");
            var corsChildren = corsSection.GetChildren().ToList();

            app.UseCors(options =>
            {
                foreach (var child in corsChildren)
                {
                    var url = child["URL"];

                    options.WithOrigins(url!)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            await InitialDataAsync(app.Services);

            app.Run();
        }

        private static async Task InitialDataAsync(IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var bogusService = scope.ServiceProvider.GetRequiredService<IBogusGenerationSrv>();

            await bogusService.CreateAdminAsync();
        }
    }
}
