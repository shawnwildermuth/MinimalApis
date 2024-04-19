namespace MinimalApis.FluentValidation;

public static class EndpointBuilderExtensions
{
  /// <summary>
  /// Extension to add an Endpoint Filter to automatically validate
  /// a model type and return a validation error if it fails before
  /// the Minimal API is executed.
  /// </summary>
  /// <typeparam name="TModel">The type to validate with FluentValidation.</typeparam>
  /// <param name="builder">The route handler builder (e.g. MapGet, MapGroup, etc.)</param>
  /// <returns>The route handler builder.</returns>
  public static RouteHandlerBuilder Validate<TModel>(this RouteHandlerBuilder builder)
      where TModel : class => builder.AddEndpointFilter<ValidationEndpointFilter<TModel>>();
}
