using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Constants;
using AIExtensionsCenter.Infrastructure.BackgroundServices;
using AIExtensionsCenter.Infrastructure.Data;
using AIExtensionsCenter.Infrastructure.Data.Interceptors;
using AIExtensionsCenter.Infrastructure.Identity;
using AIExtensionsCenter.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Minio;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        var connectionString = configuration["DB_URI"];
        Guard.Against.Null(connectionString, message: "Connection string 'DB_URI' not found.");

        builder.Services.AddTransient<IAppEmailSender, AppEmailSender>();
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        builder.Services.AddScoped<IFileStorageService, MinioFileStorageService>();
        builder.Services.AddScoped<IRubyFileService, RubyFileService>();
        builder.Services.AddHostedService<LicenseExpiryService>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        var endpoint = configuration["MINIO:Endpoint"];
        var accessKey = configuration["MINIO:AccessKey"];
        var secretKey = configuration["MINIO:SecretKey"];
        var region = configuration["MINIO:Region"];
        var useSSL = configuration.GetValue<bool>("MINIO:Usessl");

        Guard.Against.Null(endpoint, message: "Connection string 'Endpoint' not found.");
        Guard.Against.Null(accessKey, message: "Connection string 'AccessKey' not found.");
        Guard.Against.Null(secretKey, message: "Connection string 'SecretKey' not found.");
        Guard.Against.Null(region, message: "Connection string 'Region' not found.");
        Guard.Against.Null(useSSL, message: "Connection string 'Usessl' not found.");

        builder.Services.AddSingleton(new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey,secretKey)
            .WithRegion(region)
            .WithSSL(useSSL)
            .Build());

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        builder.Services
            .AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        builder.Services.AddAuthorizationBuilder();

        builder.Services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        builder.Services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
    }
}
