using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Application.Extensions.Commands.CreateExtension;
using AIExtensionsCenter.Application.Extensions.Commands.DeleteExtension;
using AIExtensionsCenter.Application.Extensions.Commands.UpdateExtension;
using AIExtensionsCenter.Application.Extensions.Commands.UploadExtensionImage;
using AIExtensionsCenter.Application.Extensions.Queries.GetExtension;
using AIExtensionsCenter.Application.Extensions.Queries.GetExtensionById;
using MediatR;

namespace AIExtensionsCenter.Web.Endpoints;
public class Extensions : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetExtensionWithPagination)
            .MapGet(GetExtensionById, "{id}")
            .MapPost(CreateExtension)
            .MapPut(UpdateExtension, "{id}")
            .MapDelete(DeleteExtension, "{id}")
            .MapPost(UploadImage, "{id}/upload-image");

    }
    private Task<PaginatedList<ExtensionVM>> GetExtensionWithPagination(ISender sender, [AsParameters] GetExtensionQuery query)
    {
        return sender.Send(query);
    }
    private async Task<IResult> GetExtensionById(ISender sender, Guid id)
    {
        var result = await sender.Send(new GetExtensionByIdQuery(id));
        return Results.Ok(result);
    }
    private Task<Guid> CreateExtension(ISender sender, CreateExtensionCommand command)
    {
        return sender.Send(command);
    }
    private async Task<IResult> UpdateExtension(ISender sender, Guid id, UpdateExtensionCommand command)
    {
        if (id != command.Id) return Results.BadRequest("The provided ID does not match the command ID.");
        await sender.Send(command);
        return Results.NoContent();
    }
    private async Task<IResult> DeleteExtension(ISender sender, Guid id)
    {
        await sender.Send(new DeleteExtensionCommand(id));
        return Results.NoContent();
    }
    private async Task<IResult> UploadImage(Guid id, IFormFile file, ISender sender, IFileStorageService fileStorage)
    {
        var command = new UploadExtensionImageCommand(id, file);
        var result = await sender.Send(command);
        return TypedResults.Ok(new { ImageUrl = result });
    }
}
