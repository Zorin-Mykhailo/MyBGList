using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using MyBGList.ValidationAttributes;

namespace MyBGList.Swagger;

public class SortOrderFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        var attributes = context.ParameterInfo?
            .GetCustomAttributes(true)
            .OfType<AllowedStringsAttribute>();

        if (attributes == null) return;
        foreach(var attribute in attributes)
            parameter.Schema.Extensions.Add("pattern", new OpenApiString(string.Join("|", attribute.AllowedVaues.Select(v => $"^{v}$"))));
    }
}