using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AIExtensionsCenter.Infrastructure.BackgroundServices;

public class LicenseExpiryService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<LicenseExpiryService> _logger;

    public LicenseExpiryService(IServiceScopeFactory serviceScopeFactory, ILogger<LicenseExpiryService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = GetDelayUntilMidnight();

            await LicenseExpiry(stoppingToken);
            await Task.Delay(delay, stoppingToken);
        }
    }

    private async Task LicenseExpiry(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            var expiredLicenses = await context.Licenses
                .Where(x => x.ExpirationDate <= DateTime.UtcNow && x.LicenseStatus == LicenseStatus.Active)
                .ExecuteUpdateAsync(setters => setters.SetProperty(l => l.LicenseStatus, LicenseStatus.Expired), stoppingToken);

            if (expiredLicenses > 0)
            {
                _logger.LogInformation("Updated {Count} expired licenses.", expiredLicenses);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating expired licenses.");
        }
    }
    private static TimeSpan GetDelayUntilMidnight()
    {
        var now = DateTime.UtcNow;
        var midnight = now.Date.AddDays(1);
        return midnight - now;
    }
}
