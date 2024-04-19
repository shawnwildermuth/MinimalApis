using FluentValidation;
using FluentValidation.Results;

namespace MinimalApis.FluentValidation;

/// <summary>
/// Endpoint Filter for Minimal APIs to automatically validate
/// a model type and return a validation error if it fails before
/// the Minimal API is executed.
/// </summary>
/// <remarks>
/// If the parameter in the API call is a collection, will return first invalid result only.
/// </remarks>
/// <typeparam name="TModel">The type to find in the API to validate with this Endpoint.</typeparam>
public class ValidationEndpointFilter<TModel> : IEndpointFilter
    where TModel : class
{
  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
      EndpointFilterDelegate next)
  {

    // Find argument of Type
    TModel? modelArgument = context.Arguments.Where(a => a?.GetType() == typeof(TModel)).FirstOrDefault() as TModel;

    // Find the validator (will throw exception, but that is ok
    var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<TModel>>();

    ValidationResult result;
    string? details = null;

    if (modelArgument is not null)
    {
      // Test the validation
      result = await validator.ValidateAsync(modelArgument);
    }
    else
    {
      // Try to search for collection of type
      var collection = context.Arguments.Where(a => a?.GetType().IsAssignableTo(typeof(IEnumerable<TModel>)) == true)
        .FirstOrDefault() as IEnumerable<TModel>;

      // If we got a collection, validate the collection
      if (collection is not null)
      {
        var results = new List<ValidationResult>();
        foreach (var item in collection)
        {
          results.Add(await validator.ValidateAsync(item));
        }

        if (results.Any())
        {
          var invalids = results.Where(r => !r.IsValid).ToList();

          if (invalids.Any())
          {
            var indexes = invalids.Select(i => results.IndexOf(i));

            // Format details
            details = $"{invalids.Count()} item(s) in the collection are invalid. Indexes of failed items: {string.Join(",", indexes)}. Only the first invalid result is included.";

            result = invalids.First();
          }
          else
          {
            result = results.First();
          }
        }
        else
        {
          // Just return the last result (the first error or success)
          result = new ValidationResult();
        }
      }
      else
      {
        throw new InvalidOperationException($"Could not find argument in the API that matches the {nameof(TModel)}");
      }
    }
 
    if (result.IsValid)
    {
      // Continue middleware
      return await next(context);
    }

    // report the validation error
    return Results.ValidationProblem(result.ToDictionary(), details ?? $"{nameof(TModel)} failed validation");
  }

}

