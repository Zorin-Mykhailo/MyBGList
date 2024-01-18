using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.Models;
using MyBGList.Swagger;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt =>
{
    var msgProvider = opt.ModelBindingMessageProvider;
    msgProvider.SetValueIsInvalidAccessor(x => $"The value '{x}' is invalid.");
    msgProvider.SetValueMustBeANumberAccessor(x => $"The field '{x}' must be a number.");
    msgProvider.SetAttemptedValueIsInvalidAccessor((x, y) => $"The value '{x}' is not valid for {y}");
    msgProvider.SetMissingKeyOrValueAccessor(() => "A value is required");
});

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(cfg => cfg.WithOrigins(builder.Configuration["AllowedOrigins"]!).AllowAnyHeader().AllowAnyMethod());
    opt.AddPolicy(name: "AnyOrigin", cfg => cfg.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt => {
    opt.ParameterFilter<SortColumnFilter>();
    opt.ParameterFilter<SortOrderFilter>();
});
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

WebApplication app = builder.Build();

if(app.Configuration.GetValue<bool>("UseSwagger")) app.UseSwagger().UseSwaggerUI();

if(app.Configuration.GetValue<bool>("UseDeveloperExceptionPage")) app.UseDeveloperExceptionPage();
else app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapGet("/error",
    [EnableCors("AnyOrigin"), ResponseCache(NoStore = true)] () =>
    Results.Problem());

app.MapGet("/error/test",
    [EnableCors("AnyOrigin"), ResponseCache(NoStore = true)] () =>
    { throw new Exception("Test"); });

app.MapGet("/cod/test", [EnableCors("AnyOrigin"), ResponseCache(NoStore = true)] () =>
    Results.Text(
        "<script>window.alert('Your client supports JavaScript!\\r\\n\\r\\n" +
        $"Server time (UTC): {DateTime.UtcNow.ToString("o")}\\r\\n" +
        "Client time (UTC): ' + new Date().toISOString());</script>" +
        "<noscript>Your client does not support JavaScript</noscript>"
        , "text/html"));

app.MapControllers().RequireCors("AnyOrigin");

app.Run();