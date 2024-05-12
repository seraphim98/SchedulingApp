using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System;
using System.Collections.Generic;
using Scheduler.Models;
using Scheduler;
/*class Startup 
{
    static void Main(string[] args) {
        var builder = WebApplication.CreateHostBuilder(args);
        var connectionString = "Host=localhost;Port=5432;Database=Scheduler;Username=postgres;Password=admin";
        builder.Services.AddControllers();
        var mapperConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });

                IMapper mapper = mapperConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);
        builder.Services.AddMvc();
        builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }    
}*/