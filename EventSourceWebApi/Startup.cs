﻿using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.DataContext;
using EventSourceWebApi.DataContext.Repositories;
using EventSourceWebApi.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourceWebApi
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
            services.AddEntityFrameworkNpgsql().AddDbContext<EventSourceDbContext>(opt =>
           opt.UseNpgsql(Configuration.GetConnectionString("EventSourceDbConnection")));

            services.AddMvc();

            services.AddTransient<IEventsService, EventsService>();
            services.AddTransient<IEventsRepository, EventsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
