using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace AppointmentApplication.Api.OpenApi.Transformers;

internal sealed class VersionInfoTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        string version = context.DocumentName;

        document.Info.Version = version;
        document.Info.Title = $"MechanicShop API {version}";

        return Task.CompletedTask;
    }
}