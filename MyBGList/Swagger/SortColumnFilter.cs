using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using MyBGList.ValidationAttributes;
using MyBGList.DTO;

namespace MyBGList.Swagger;

public class SortColumnFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        var attributes = context.ParameterInfo?
            .GetCustomAttributes(true)
            .OfType<NameOfPropertyAttribute>();

        if (attributes == null) return;
        foreach (var attribute in attributes)
        {
            var patternValues = string.Join("|", attribute.EntityProperties.Select(p => $"^{p}$"));
            parameter.Schema.Extensions.Add("pattern", new OpenApiString(patternValues));
        }
    }
}