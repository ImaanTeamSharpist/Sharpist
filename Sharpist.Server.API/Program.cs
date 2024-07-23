
using Microsoft.EntityFrameworkCore;
using Sharpist.Server.API.Extensions;
using Sharpist.Server.API.Models;
using Sharpist.Server.Data.Contexts;
using Sharpist.Server.Service.Helpers;
using Sharpist.Server.Service.Mappers;

namespace Sharpist.Server.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // Configure Servervice
            builder.Services.AddCustomServices();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            // CORS
            builder.Services.ConfigureCors();

            // swagger set up
            builder.Services.AddSwaggerService();
            // JWT service
            builder.Services.AddJwtService(builder.Configuration);

            var app = builder.Build();
            WebHostEnviromentHelper.WebRootPath = Path.GetFullPath("wwwroot");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
