using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using MinimalApis.FluentValidation;

namespace MinimalApis.FluentValidation.Tests;

public class ValidationEndpointFilterTests
{
  [Fact]
  public async void CanValidateValidModel()
  {
    var context = BuildContext(new TestModel()
    {
      FirstName = "Shawn",
      LastName = "Wildermuth",
      Email = "shawn@aol.com"
    });

    var endpoint = new ValidationEndpointFilter<TestModel>();
    var result = await endpoint.InvokeAsync(context, Next);
    Assert.IsType<Ok>(result);
  }

  [Fact]
  public async void CanInvalidateInvalidModel()
  {
    var context = BuildContext(new TestModel());

    var endpoint = new ValidationEndpointFilter<TestModel>();
    var result = await endpoint.InvokeAsync(context, Next);
    Assert.IsType<ProblemHttpResult>(result);
  }

  [Fact]
  public async void CanValidateValidArrayModel()
  {
    var context = BuildContext(new[]
    {
      new TestModel()
      {
        FirstName = "Shawn",
        LastName = "Wildermuth",
        Email = "shawn@aol.com"
      },
      new TestModel()
      {
        FirstName = "Shawn",
        LastName = "Wildermuth",
        Email = "shawn@aol.com"
      }
    });

    var endpoint = new ValidationEndpointFilter<TestModel>();
    var result = await endpoint.InvokeAsync(context, Next);
    Assert.IsType<Ok>(result);
  }

  [Fact]
  public async void CanInvalidateInvalidArrayModel()
  {
    var context = BuildContext(new[]
    {
      new TestModel(),
      new TestModel()
      {
        FirstName = "Shawn",
        LastName = "Wildermuth",
        Email = "shawn@aol.com"
      }
    });

    var endpoint = new ValidationEndpointFilter<TestModel>();
    var result = await endpoint.InvokeAsync(context, Next);
    Assert.IsType<ProblemHttpResult>(result);
  }

  EndpointFilterDelegate Next = (EndpointFilterInvocationContext ctx) => new ValueTask<object?>(Results.Ok());

  EndpointFilterInvocationContext BuildContext<T>(T model)
  {
    var svcs = new ServiceCollection();
    svcs.AddTransient<IValidator<TestModel>, TestModelValidator>();

    var provider = svcs.BuildServiceProvider();
    HttpContext http = new DefaultHttpContext();
    http.RequestServices = provider;

    return EndpointFilterInvocationContext.Create(http, model);
  }
}