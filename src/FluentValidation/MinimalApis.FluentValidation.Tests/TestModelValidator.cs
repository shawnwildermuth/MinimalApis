using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MinimalApis.FluentValidation.Tests;

public class TestModelValidator : AbstractValidator<TestModel>
{
  public TestModelValidator()
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
