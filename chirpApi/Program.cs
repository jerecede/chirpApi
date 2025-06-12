using chirpApi.Models;
using chirpApi.Services.Services;
using chirpApi.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Serilog;

namespace chirpApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(builder.Configuration)
                            .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v3", new OpenApiInfo
                {
                    Title = "Cinguettio API",
                    Version = "v3",
                    Description = "API for the Cinguettio application, a social media platform for sharing short messages."
                });
            });

            // Add services to the container
            builder.Services.AddDbContext<CinguettioContext>(options => // è un AddScoped mascherato
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))); //non c'è bisogno di AppConfig

            builder.Services.AddControllers();

            // dependencies injection, stiamo registrando, aggiungendo al conteiner, conviene senno dovremmo scrivere troppo codice per ogni controller, ingestibile
            builder.Services.AddScoped<IChirpsService, JereChirpsService>(); //dura dall'inizio della richiesta fino alla fine della richiesta, standard
            //builder.Services.AddSingleton<IChirpsService, JereChirpsService>(); //dura tutto il ciclo di vita dell'applicazione
            //builder.Services.AddTransient<IChirpsService, JereChirpsService>(); //dura solo per il tempo di esecuzione del metodo, ricreato ogni volta che viene chiamato
            builder.Services.AddScoped<ICommentsService, JereCommentsService>();

            var app = builder.Build();

            app.UseSwagger(c =>
            {
                c.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v3/swagger.json", "Cinguettio API V1");
            });

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
