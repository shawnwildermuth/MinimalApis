# MinimalApis.FluentValidation

![Build and Test](https://github.com/shawnwildermuth/MinimalApis/actions/workflows/validation.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/MinimalApis.FluentValidation)](https://nuget.org/packages/MinimalApis.FluentValidation) [![Nuget](https://img.shields.io/nuget/dt/MinimalApis.FluentValidation
)](https://nuget.org/packages/MinimalApis.FluentValidation)

To install the package:

```
dotnet add package MinimalApis.FluentValidation
```


I'm a big fan of how Fluent Validation works, but as I was teaching Minimal APIs - it was tedious to add validation. In .NET 7, Microsoft introduced Endpoint Filters as a good solution.

Since then, I've been using endpoint filters to execute Fluent Validation's Validators. The result is this library. Assuming you've already used Fluent Validation to create your validators, you can simply call Validate to run the validation. For example:

```cs
app.MapPost("/test", (SomeModel model) => Results.Created("/test/1", model))
  .Validate<SomeModel>();
```

The call to Validate is an extension method that works on MapGroup, MapPost, MapPut, and MapDelete. (I don't prevent it's usage on MapGet or MapDelete, but they don't really make sense there. 

Essentially, the call to validate will search for a Validator class for the provided type (`TModel`). If it doesn't find it, it will throw an exception. If it does find the validator, it will find find any parameters that match the provided type and validate it using the validator. 

Finally, if there are any validation errors, it returns a `ValidationProblem`:

```cs
Results.ValidationProblem(...);
```

> Note, the validators are called asynchronously.

You can also use the endpoint filter without the extension method if you like:

```cs
app.MapPut("/test", (SomeModel model) => Results.Ok(model))
  .AddEndpointFilter<ValidationEndpointFilter<SomeModel>>();
```

This might be useful if you need to subclass the `ValidaitonEndpointFilter`.

Please submit any issues and/or pull requests if you have questions or problems.
