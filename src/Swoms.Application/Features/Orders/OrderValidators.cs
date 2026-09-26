using FluentValidation;

namespace Swoms.Application.Features.Orders;

public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(request => request.WarehouseId).NotEmpty();
        RuleFor(request => request.Customer).NotNull().SetValidator(new CustomerRequestValidator());
        RuleFor(request => request.Items).NotEmpty();
        RuleForEach(request => request.Items).SetValidator(new CreateOrderItemRequestValidator());
    }
}

public sealed class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    public CustomerRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty().MaximumLength(200);
        RuleFor(request => request.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(request => request.Line1).NotEmpty().MaximumLength(300);
        RuleFor(request => request.Line2).MaximumLength(300);
        RuleFor(request => request.City).NotEmpty().MaximumLength(100);
        RuleFor(request => request.State).NotEmpty().MaximumLength(100);
        RuleFor(request => request.PostalCode).NotEmpty().MaximumLength(32);
        RuleFor(request => request.Country).NotEmpty().MaximumLength(100);
    }
}

public sealed class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
{
    public CreateOrderItemRequestValidator()
    {
        RuleFor(request => request.ProductId).NotEmpty();
        RuleFor(request => request.Quantity).GreaterThan(0);
    }
}
