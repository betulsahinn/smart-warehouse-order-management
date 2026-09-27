using AutoMapper;
using Swoms.Application.Common.Exceptions;
using Swoms.Application.Common.Interfaces;
using Swoms.Domain.Entities;
using Swoms.Domain.Enums;
using Swoms.Domain.ValueObjects;

namespace Swoms.Application.Features.Orders;

public sealed class OrderService : IOrderService
{
    private readonly IRepository<Customer> _customers;
    private readonly IRepository<Order> _orders;
    private readonly IRepository<Product> _products;
    private readonly IRepository<StockItem> _stockItems;
    private readonly IRepository<StockMovement> _stockMovements;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(
        IRepository<Customer> customers,
        IRepository<Order> orders,
        IRepository<Product> products,
        IRepository<StockItem> stockItems,
        IRepository<StockMovement> stockMovements,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _customers = customers;
        _orders = orders;
        _products = products;
        _stockItems = stockItems;
        _stockMovements = stockMovements;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orders.ListAsync(cancellationToken: cancellationToken, includes: nameof(Order.Items));
        return _mapper.Map<IReadOnlyList<OrderDto>>(orders);
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.ExecuteInTransactionAsync(async transactionCancellationToken =>
        {
            var customer = new Customer(
                request.Customer.Name,
                request.Customer.Email,
                new Address(
                    request.Customer.Line1,
                    request.Customer.Line2,
                    request.Customer.City,
                    request.Customer.State,
                    request.Customer.PostalCode,
                    request.Customer.Country));

            _customers.Add(customer);

            var pendingOrder = new Order(customer.Id, request.WarehouseId);

            foreach (var item in request.Items)
            {
                var product = await _products.GetByIdAsync(item.ProductId, transactionCancellationToken)
                    ?? throw new NotFoundException(nameof(Product), item.ProductId);

                var stockItem = await _stockItems.FirstOrDefaultAsync(
                    stock => stock.ProductId == item.ProductId && stock.WarehouseId == request.WarehouseId,
                    transactionCancellationToken)
                    ?? throw new NotFoundException(nameof(StockItem), $"{item.ProductId}/{request.WarehouseId}");

                stockItem.Reserve(item.Quantity);
                pendingOrder.AddItem(product.Id, product.Name, item.Quantity, product.UnitPrice);
                _stockMovements.Add(new StockMovement(product.Id, request.WarehouseId, -item.Quantity, StockMovementType.Reservation, $"Reserved for {pendingOrder.OrderNumber}"));
            }

            pendingOrder.Confirm();
            _orders.Add(pendingOrder);

            return pendingOrder;
        }, cancellationToken);

        return _mapper.Map<OrderDto>(order);
    }
}
