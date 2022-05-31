
using MassTransit;
using MassTransit.Definition;
using Mozart.Play.Catalog.Service.Entities;
using Mozart.Play.Common.MassTransit;
using Mozart.Play.Common.MongoDb;
using Mozart.Play.Common.Settings;

namespace Mozart.Play.Catalog.Service
{
    public class Program
    {
        private const string AllowedOriginSetting = "AllowedOrigin";
        private static ServiceSettings serviceSettings;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;

            // Add services to the container.
            serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            services.AddMongo()
                    .AddMongoRepository<Item>("items")
                    .AddMassTransitWithRabbitMQ();

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseCors(builder =>
                {
                    builder.WithOrigins(configuration[AllowedOriginSetting])
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}