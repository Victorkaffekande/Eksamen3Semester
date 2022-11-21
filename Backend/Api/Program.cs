using System.Text.Json.Serialization;
using Application.DTOs;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite("Data source=db.db"));


builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
//Making a mapper configuration
var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<UserRegisterDTO, User>();
    cfg.CreateMap<PatternDTO, Pattern>();
});

var mapper = config.CreateMapper();

builder.Services.AddSingleton(mapper);

Application.DependencyResolver.DependencyResolverService.RegisterApplicationLayer(builder.Services);
infrastructure.DependencyResolver.DependencyResolverService.RegisterInfrastructure(builder.Services);


var app = builder.Build();

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