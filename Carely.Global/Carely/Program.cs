
using Carely.Data;
using Carely.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security;
using System.Text;

namespace Carely
{
    public class Program
    {
        public static void Main(string[] args)
        {

            #region DI

            #region CreateWebApplication
            var builder = WebApplication.CreateBuilder(args);
            #endregion

            #region UserDefine

            #region DataBase
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            #region Security

            #region Jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("JwtSettings");

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)
                    )
                };
            });
            #endregion

            #region Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                Array.Empty<string>()
                }
                });
            });
            #endregion

            builder.Services.AddAuthorization(options =>
            {
            });
            #endregion

            #region Validation

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            #endregion

            #region RepoPattern

            builder.Services.AddTransient<IJwtTokenProvider, JwtTokenProvider>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IMedicationRepository, MedicationRepository>();
            builder.Services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddTransient<IClinicRepository,ClinicRepository>();
            builder.Services.AddTransient<IMeetingRepository, MeetingRepository>();

            #endregion

            #endregion

            #region DuiltIn
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            #region BuildWebApp
            var app = builder.Build();
            #endregion

            #endregion

            #region MW
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty; 
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
