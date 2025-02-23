using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Application.Licenses.Commands.ActivateLicense;
using AIExtensionsCenter.Application.Licenses.Commands.CreateLicense;
using AIExtensionsCenter.Application.Licenses.Commands.DeActivateLicense;
using AIExtensionsCenter.Application.Licenses.Commands.DeleteLicense;
using AIExtensionsCenter.Application.Licenses.Commands.UpdateLicense;
using AIExtensionsCenter.Application.Licenses.Queries.GetLicenseByExtensionId;
using MediatR;

namespace AIExtensionsCenter.Web.Endpoints;

public class Licenses : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetLicenseByExtensionId, "/extension")
            .MapPost(CreateLicense)
            .MapPut(UpdateLicense, "{id}")
            .MapDelete(DeleteLicense, "{id}")
            .MapPost(ActivateLicense, "{id}/activate")
            .MapPost(DeActivateLicense, "{id}/deactivate");
    }
    private Task<PaginatedList<LicenseVM>> GetLicenseByExtensionId(ISender sender, [AsParameters] GetLicenseByExtensionIdQuery query)
    {
        return sender.Send(query);
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
    private async Task<IResult> ActivateLicense(ISender sender, Guid id, ActivateLicenseCommand command)
    {
        if (id != command.Id) return Results.BadRequest("The provided ID does not match the command ID.");
        await sender.Send(command);
        return Results.NoContent();
    }
    private async Task<IResult> DeActivateLicense(ISender sender, Guid id, DeActivateLicenseCommand command)
    {
        if (id != command.Id) return Results.BadRequest("The provided ID does not match the command ID.");
        await sender.Send(command);
        return Results.NoContent();
    }
}
