using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using MinimalApis.Discovery;

namespace MinimalApiDiscovery.Tests;

public class DumbApi : IApi
{
  public void Register(IEndpointRouteBuilder builder)
  {
    builder.MapGet("/api", Get);
  }

  static IResult Get() => Results.Ok("Works");
}
