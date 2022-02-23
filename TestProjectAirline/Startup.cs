using Bussiness;
using DataAccess;
using DataAccess.Repository;
using DataAccess.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestProjectAirline
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            var serviceProvider = services.BuildServiceProvider();
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
     
            services.AddSingleton(typeof(ILogger), serviceProvider.GetService<ILogger<JourneyRepository>>()); 
            services.AddSingleton(typeof(ILogger), serviceProvider.GetService<ILogger<FlightsRepository>>());
            services.AddSingleton(typeof(ILogger), serviceProvider.GetService<ILogger<TransportRepository>>());
            services.AddSingleton(typeof(ILogger), serviceProvider.GetService<ILogger<FlightsServices>>());
            services.AddSingleton(typeof(ILogger), serviceProvider.GetService<ILogger<NewShoreServices>>());

            services.AddScoped<HttpClient>();
            services.AddScoped<AccessSettings>();
            services.AddScoped<NewShoreServices>();
            services.AddScoped<TransportRepository>();
            services.AddScoped<FlightsRepository>();
            services.AddScoped<JourneyRepository>();
            services.AddScoped<FlightsServices>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flights", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                builder.AllowAnyHeader()
                .AllowAnyMethod().AllowAnyOrigin());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flights v1"));

            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flights v1");
                });
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run(context =>
            {
                context.Response.Redirect("swagger");
                return Task.CompletedTask;
            });
        }
    }
}
