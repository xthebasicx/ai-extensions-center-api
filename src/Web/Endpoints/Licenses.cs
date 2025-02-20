using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Application.Licenses.Commands.ActivateLicense;
using AIExtensionsCenter.Application.Licenses.Commands.CreateLicense;
using AIExtensionsCenter.Application.Licenses.Commands.DeleteLicense;
using AIExtensionsCenter.Application.Licenses.Commands.UpdateLicense;
using AIExtensionsCenter.Application.Licenses.Queries.GetLicense;
using AIExtensionsCenter.Application.Licenses.Queries.GetLicenseById;
using MediatR;

namespace AIExtensionsCenter.Web.Endpoints;

public class Licenses : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetLicenseWithPagination)
            .MapGet(GetLicenseById, "{id}")
            .MapPost(CreateLicense)
            .MapPut(UpdateLicense, "{id}")
            .MapDelete(DeleteLicense, "{id}")
            .MapPost(ActivateLicense, "{id}/activate");
    }
    private Task<PaginatedList<LicenseVM>> GetLicenseWithPagination(ISender sender, [AsParameters] GetLicenseQuery query)
    {
        return sender.Send(query);
    }
    private async Task<IResult> GetLicenseById(ISender sender, Guid id)
    {
        var result = await sender.Send(new GetLicenseByIdQuery(id));
        return Results.Ok(result);
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
}
