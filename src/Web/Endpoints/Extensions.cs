using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Application.Extensions.Commands.CreateExtension;
using AIExtensionsCenter.Application.Extensions.Commands.DeleteExtension;
using AIExtensionsCenter.Application.Extensions.Commands.UpdateExtension;
using AIExtensionsCenter.Application.Extensions.Commands.UploadExtensionImage;
using AIExtensionsCenter.Application.Extensions.Queries.GetExtension;
using AIExtensionsCenter.Application.Extensions.Queries.GetExtensionById;
using AIExtensionsCenter.Application.Extensions.Queries.GetExtensionByUserId;
using MediatR;

namespace AIExtensionsCenter.Web.Endpoints;
public class Extensions : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetExtensionWithPagination)
            .MapGet(GetExtensionByUserId, "user")
            .MapGet(GetExtensionById, "{id}")
            .MapPost(CreateExtension)
            .MapPut(UpdateExtension, "{id}")
            .MapDelete(DeleteExtension, "{id}")
            .MapPost(UploadImage, "upload-image");

    }
    private Task<PaginatedList<ExtensionVM>> GetExtensionWithPagination(ISender sender, [AsParameters] GetExtensionQuery query)
    {
        return sender.Send(query);
    }
    private Task<PaginatedList<ExtensionVM>> GetExtensionByUserId(ISender sender, [AsParameters] GetExtensionByUserIdQuery query)
    {
        return sender.Send(query);
    }
    private Task<ExtensionVM> GetExtensionById(ISender sender, Guid id)
    {
        return sender.Send(new GetExtensionByIdQuery(id));
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
    private async Task<IResult> UploadImage(IFormFile file, ISender sender)
    {
        var command = new UploadExtensionImageCommand(file);
        var result = await sender.Send(command);
        return Results.Ok(result);
    }
}
