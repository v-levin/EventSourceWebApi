﻿using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.DataContext;
using EventSourceWebApi.DataContext.Repositories;
using EventSourceWebApi.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace EventSourceWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
             .Enrich.FromLogContext()
             .WriteTo.Console()
             .WriteTo.File("log.txt")
             .CreateLogger();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddEntityFrameworkNpgsql().AddDbContext<EventSourceDbContext>(opt =>
           opt.UseNpgsql(Configuration.GetConnectionString("EventSourceDbConnection")));

            services.AddMvc();

            services.AddSingleton(Log.Logger);
            services.AddTransient<IEventsService, EventsService>();
            services.AddTransient<IEventsRepository, EventsRepository>();
            services.AddTransient<IPlacesRepository, PlacesRepository>();
            services.AddTransient<IPlacesService, PlacesService>();

            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new Info { Title = "EventSource API", Version = "v1" });
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventSource API V1");
            });
        }
    }
}
