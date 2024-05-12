using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Scheduler;
using Scheduler.Database;


var builder = WebApplication.CreateBuilder(args);
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