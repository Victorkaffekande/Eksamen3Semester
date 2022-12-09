using System.Text;
using System.Text.Json.Serialization;
using Application.DTOs;
using Application.DTOs.Like;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
    cfg.CreateMap<PatternUpdateDTO, Pattern>();
    cfg.CreateMap<ProjectCreateDTO, Project>();
    cfg.CreateMap<PostCreateDTO, Post>();
    cfg.CreateMap<PostUpdateDTO, Post>();
    cfg.CreateMap<Pattern, PatternGetAllDTO>();
    cfg.CreateMap<UserDTO, User>();
    cfg.CreateMap<User, UserDTO>();
    cfg.CreateMap<SimpleLikeDto, Like>();
    cfg.CreateMap<Like, SimpleLikeDto>();
    cfg.CreateMap<DashboardPostDTO, Like>();
    cfg.CreateMap<Post, PostFromProjectDTO>();
    cfg.CreateMap<Pattern, PatternGetAllDTO>();
    cfg.CreateMap<Like, UserDTO>().ForMember(p => p.Id, opt => opt.MapFrom(p =>p.LikedUser.Id))
        .ForMember(p => p.Username, opt => opt.MapFrom(p =>p.LikedUser.Username))
        .ForMember(p => p.Email, opt => opt.MapFrom(p =>p.LikedUser.Email))
        .ForMember(p => p.ProfilePicture, opt => opt.MapFrom(p =>p.LikedUser.ProfilePicture))
        .ForMember(p => p.BirthDay, opt => opt.MapFrom(p =>p.LikedUser.BirthDay));
    cfg.CreateMap<Like, User>().ForMember(p => p.Id, opt => opt.MapFrom(p =>p.LikedUser.Id))
        .ForMember(p => p.Username, opt => opt.MapFrom(p =>p.LikedUser.Username))
        .ForMember(p => p.Email, opt => opt.MapFrom(p =>p.LikedUser.Email))
        .ForMember(p => p.ProfilePicture, opt => opt.MapFrom(p =>p.LikedUser.ProfilePicture))
        .ForMember(p => p.BirthDay, opt => opt.MapFrom(p =>p.LikedUser.BirthDay));

});


var mapper = config.CreateMapper();

builder.Services.AddSingleton(mapper);

Application.DependencyResolver.DependencyResolverService.RegisterApplicationLayer(builder.Services);
infrastructure.DependencyResolver.DependencyResolverService.RegisterInfrastructure(builder.Services);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetValue<String>("AppSettings:Secret")))
    };
});
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("AdminPolicy", (policy) => { policy.RequireRole("admin"); });
});
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

