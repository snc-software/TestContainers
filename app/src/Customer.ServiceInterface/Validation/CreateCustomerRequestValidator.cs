using Customer.ServiceInterface.Requests;
using FluentValidation;

namespace Customer.ServiceInterface.Validation;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(p => p.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100).WithMessage("'Name' must be less than 100 characters.");

        RuleFor(p => p.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .MaximumLength(150).WithMessage("'Email' must be less than 150 characters.")
            .EmailAddress();
    }
}