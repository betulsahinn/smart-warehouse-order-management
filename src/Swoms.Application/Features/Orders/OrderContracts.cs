using Swoms.Domain.Enums;

namespace Swoms.Application.Features.Orders;

public sealed record CreateOrderRequest(
    Guid WarehouseId,
    CustomerRequest Customer,
    IReadOnlyCollection<CreateOrderItemRequest> Items);

public sealed record CustomerRequest(
    string Name,
    string Email,
    string Line1,
    string? Line2,
    string City,
    string State,
    string PostalCode,
    string Country);

public sealed record CreateOrderItemRequest(Guid ProductId, int Quantity);

public sealed record OrderDto(
    Guid Id,
    string OrderNumber,
    Guid CustomerId,
    Guid WarehouseId,
    OrderStatus Status,
    decimal TotalAmount,
    IReadOnlyCollection<OrderItemDto> Items);

public sealed record OrderItemDto(Guid Id, Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal LineTotal);

public interface IOrderService
{
    Task<IReadOnlyList<OrderDto>> GetOrdersAsync(CancellationToken cancellationToken = default);

    Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
}
