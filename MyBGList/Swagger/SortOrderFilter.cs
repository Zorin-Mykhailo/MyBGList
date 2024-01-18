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
            .Union(context.ParameterInfo.ParameterType.GetProperties().Where(p => p.Name == parameter.Name).SelectMany(p => p.GetCustomAttributes(true)))
            .OfType<AllowedStringsAttribute>();

        if (attributes == null) return;
        foreach(var attribute in attributes)
        {
            var patternValues = string.Join("|", attribute.AllowedVaues.Select(v => $"^{v}$"));
            parameter.Schema.Extensions.Add("pattern", new OpenApiString(patternValues));
        }
    }
}