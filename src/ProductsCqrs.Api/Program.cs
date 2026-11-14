using System.Text.Encodings.Web;
using System.Text.Unicode;
using DotNetEnv;
using ProductsCqrs.Api.Endpoints;
using ProductsCqrs.Api.ExceptionHandling;
using ProductsCqrs.Api.RateLimiting;
using ProductsCqrs.Application;
using ProductsCqrs.Infrastructure;
using Scalar.AspNetCore;

Env.NoClobber().TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement));

builder.Services.AddOpenApi(options => options.AddDocumentTransformer((document, _, _) =>
{
    document.Info.Title = "API de Productos";
    document.Info.Description = "Catálogo de productos construido con Clean Architecture y CQRS. Código fuente: https://github.com/fabrizio-fatecha/clt-bootcamp";
    return Task.CompletedTask;
}));
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddIpRateLimiting(builder.Configuration);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseRateLimiter();

app.MapOpenApi();
app.MapScalarApiReference(options => options.WithTitle("API de Productos"));

app.MapProductEndpoints();

app.Run();
