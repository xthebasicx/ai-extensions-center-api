
using AIExtensionsCenter.Application.APIKeys.Commands.CreateAPIKey;
using AIExtensionsCenter.Application.APIKeys.Commands.DeleteAPIKey;
using AIExtensionsCenter.Application.APIKeys.Queries.GetAPIKeyByUserId;
using AIExtensionsCenter.Application.Common.Models;
using MediatR;
namespace AIExtensionsCenter.Web.Endpoints;

public class APIKeys : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetAPIKeyByUserID, "user")
            .MapPost(CreateAPIKey)
            .MapDelete(DeleteAPIKey, "{id}");
    }
    private Task<APIKeyVM> GetAPIKeyByUserID(ISender sender)
    {
        var query = new GetAPIKeyByUserIdQuery();
        return sender.Send(query);
    }
    private Task<Guid> CreateAPIKey(ISender sender)
    {
        var command = new CreateAPIKeyCommand();
        return sender.Send(command);
    }
    private async Task<IResult> DeleteAPIKey(ISender sender, Guid id)
    {
        await sender.Send(new DeleteAPIKeyCommand(id));
        return Results.NoContent();
    }
}
