
using Carely.Data;
using Microsoft.EntityFrameworkCore;

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
            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
