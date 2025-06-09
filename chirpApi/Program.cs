using chirpApi.Models;
using Microsoft.EntityFrameworkCore;

namespace chirpApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddDbContext<CinguettioContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))); //non c'è bisogno di AppConfig

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
