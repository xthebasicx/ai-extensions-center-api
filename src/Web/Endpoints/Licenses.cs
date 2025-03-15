using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Application.Licenses.Commands.ActivateLicense;
using AIExtensionsCenter.Application.Licenses.Commands.CheckExpirationLicense;
using AIExtensionsCenter.Application.Licenses.Commands.CreateLicense;
using AIExtensionsCenter.Application.Licenses.Commands.DeActivateLicense;
using AIExtensionsCenter.Application.Licenses.Commands.DeleteLicense;
using AIExtensionsCenter.Application.Licenses.Commands.UpdateLicense;
using AIExtensionsCenter.Application.Licenses.Commands.ValidateLicense;
using AIExtensionsCenter.Application.Licenses.Queries.GetLicenseByExtensionId;
using MediatR;

namespace AIExtensionsCenter.Web.Endpoints;

public class Licenses : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetLicenseByExtensionId, "extension/{id}")
            .MapPost(CreateLicense)
            .MapPut(UpdateLicense, "{id}")
            .MapDelete(DeleteLicense, "{id}")
            .MapPost(DeActivateLicense, "{id}/deactivate")
            .MapPost(CheckExpirationLicense, "check-expiration");
        app.MapGroup(this)
            .MapPost(ActivateLicense, "activate")
            .MapPost(ValidateLicense, "validate");

    }
    private Task<List<LicenseVM>> GetLicenseByExtensionId(ISender sender, Guid id)
    {
        return sender.Send(new GetLicenseByExtensionIdQuery(id));
    }
    private Task<Guid> CreateLicense(ISender sender, CreateLicenseCommand command)
    {
        return sender.Send(command);
    }
    private async Task<IResult> UpdateLicense(ISender sender, Guid id, UpdateLicenseCommand command)
    {
        if (id != command.Id) return Results.BadRequest("The provided ID does not match the command ID.");
        await sender.Send(command);
        return Results.NoContent();
    }
    private async Task<IResult> DeleteLicense(ISender sender, Guid id)
    {
        await sender.Send(new DeleteLicenseCommand(id));
        return Results.NoContent();
    }
    private async Task<IResult> ActivateLicense(ISender sender, ActivateLicenseCommand command)
    {
        await sender.Send(command);
        return Results.Ok();
    }
    private async Task<IResult> DeActivateLicense(ISender sender, Guid id, DeActivateLicenseCommand command)
    {
        if (id != command.Id) return Results.BadRequest("The provided ID does not match the command ID.");
        await sender.Send(command);
        return Results.NoContent();
    }
    private async Task<IResult> CheckExpirationLicense(ISender sender)
    {
        await sender.Send(new CheckExpirationLicenseCommand());
        return Results.NoContent();
    }
    private async Task<IResult> ValidateLicense(ISender sender, ValidateLicenseCommand command)
    {
        await sender.Send(command);
        return Results.Ok();
    }
}
