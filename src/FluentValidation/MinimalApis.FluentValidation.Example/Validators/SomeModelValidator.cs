using FluentValidation;

namespace MinimalApis.FluentValidation.Example.Validators;

public class SomeModelValidator : AbstractValidator<SomeModel>
{
  public SomeModelValidator()
  {
    RuleFor(m => m.FirstName)
      .NotEmpty()
      .MinimumLength(2);

    RuleFor(m => m.LastName)
      .NotEmpty()
      .MinimumLength(2);

    RuleFor(m => m.Email)
      .EmailAddress();
  }
}
