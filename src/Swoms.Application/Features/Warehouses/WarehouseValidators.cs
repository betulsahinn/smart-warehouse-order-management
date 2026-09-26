using FluentValidation;

namespace Swoms.Application.Features.Warehouses;

public sealed class CreateWarehouseRequestValidator : AbstractValidator<CreateWarehouseRequest>
{
    public CreateWarehouseRequestValidator()
    {
        RuleFor(request => request.Code).NotEmpty().MaximumLength(32);
        RuleFor(request => request.Name).NotEmpty().MaximumLength(200);
        RuleFor(request => request.Line1).NotEmpty().MaximumLength(300);
        RuleFor(request => request.Line2).MaximumLength(300);
        RuleFor(request => request.City).NotEmpty().MaximumLength(100);
        RuleFor(request => request.State).NotEmpty().MaximumLength(100);
        RuleFor(request => request.PostalCode).NotEmpty().MaximumLength(32);
        RuleFor(request => request.Country).NotEmpty().MaximumLength(100);
    }
}
