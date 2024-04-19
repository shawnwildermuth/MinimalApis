using UsingMinimalApiDiscovery.Data;
using MinimalApis.Discovery;

namespace UsingMinimalApiDiscovery.Apis;

public class StateApi : IApi
{
  public void Register(IEndpointRouteBuilder builder)
  {
    builder.MapGet("/api/states", (StateCollection states) =>
    {
      return states;
    });
  }
}