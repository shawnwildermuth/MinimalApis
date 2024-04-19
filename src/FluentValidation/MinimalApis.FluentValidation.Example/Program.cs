using System.Reflection;
using FluentValidation;
using WilderMinds.MinimalApis.FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly());

var app = builder.Build();

app.MapPost("/test", (SomeModel model) => Results.Created("/test/1", model))
  .Validate<SomeModel>();

app.MapPost("/test/collection", (SomeModel[] model) => Results.Created("/test/collection/1", model))
  .Validate<SomeModel>();

app.MapPut("/test", (SomeModel model) => Results.Ok(model))
  .AddEndpointFilter<ValidationEndpointFilter<SomeModel>>();

app.Run();


