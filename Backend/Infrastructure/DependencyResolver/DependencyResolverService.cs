using System.Collections;
using Application;
using Application.Interfaces;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.DependencyResolver;

public static class DependencyResolverService
{
    public static void RegisterInfrastructure(IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IPatternRepository, PatternRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
    }
}