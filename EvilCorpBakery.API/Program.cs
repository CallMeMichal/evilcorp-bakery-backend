
using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Extensions;
using EvilCorpBakery.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

namespace EvilCorpBakery.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigurationManager menager = builder.Configuration;
            var co = menager.GetConnectionString("DefaultConnection");
            builder.Services.AddControllers();
            builder.Services.AddDbContext<EvilCorpBakeryAppDbContext>(options => options.UseSqlServer(menager.GetConnectionString("DefaultConnection")));
            //builder.Services.AddDbContext<EvilCorpBakeryAppDbContext>(options => options.UseInMemoryDatabase("Bakery"));


            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            //builder.Services.AddProblemDetails(); // dodane


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("super-tajny-klucz-evilcorp-bakery-2024-minimum-32-znaki")),
                    ValidateIssuer = true,
                    ValidIssuer = "EvilCorpBakery",
                    ValidateAudience = true,
                    ValidAudience = "https://evilcorpbakery.com",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "http://localhost:4200") // Angular dev server URL
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EvilCorpBakeryAppDbContext>();
                DatabaseInitializer.Initialize(context);
            }


            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(); // This adds Scalar at /scalar/v1

                // Auto-launch browser
                app.Lifetime.ApplicationStarted.Register(() =>
                {
                    var urls = app.Urls;
                    if (urls.Any())
                    {
                        var baseUrl = urls.First();
                        var scalarUrl = $"{baseUrl}/scalar/v1";

                        try
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = scalarUrl,
                                UseShellExecute = true
                            });
                        }
                        catch { }
                    }
                });
            }

            //app.UseHttpsRedirection();

            app.UseCors("AllowAngular");
            app.UseApiResponseMiddleware();
            app.UseAuthentication();
            app.UseAuthMiddleware();
            app.UseAuthorization();

            
            


            app.MapControllers();

            app.Run();
        }
    }
}
