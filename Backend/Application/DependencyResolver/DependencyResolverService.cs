﻿using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyResolver;

public static class DependencyResolverService
{
    public static void RegisterApplicationLayer(IServiceCollection service)
    {
        service.AddScoped<IAuthService, AuthService>();
        service.AddScoped<IPatternService, PatternService>();
        service.AddScoped<IProjectService, ProjectService>();
        service.AddScoped<IPostService, PostService>();
        service.AddScoped<IUserService, UserService>();
    }
}