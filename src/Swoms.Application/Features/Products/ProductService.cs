using AutoMapper;
using Swoms.Application.Common.Exceptions;
using Swoms.Application.Common.Interfaces;
using Swoms.Domain.Entities;
using Swoms.Domain.Enums;

namespace Swoms.Application.Features.Products;

public sealed class ProductService : IProductService
{
    private readonly IRepository<Product> _products;
    private readonly IRepository<StockItem> _stockItems;
    private readonly IRepository<StockMovement> _stockMovements;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(
        IRepository<Product> products,
        IRepository<StockItem> stockItems,
        IRepository<StockMovement> stockMovements,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _products = products;
        _stockItems = stockItems;
        _stockMovements = stockMovements;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _products.ListAsync(cancellationToken: cancellationToken);
        return _mapper.Map<IReadOnlyList<ProductDto>>(products);
    }

    public async Task<ProductDto> GetProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), id);

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = new Product(request.Sku, request.Name, request.Description, request.UnitPrice, request.ReorderLevel);

        _products.Add(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), id);

        product.Update(request.Name, request.Description, request.UnitPrice, request.ReorderLevel);
        _products.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductDto>(product);
    }

    public async Task AdjustStockAsync(Guid productId, AdjustStockRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _products.GetByIdAsync(productId, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), productId);

        var stockItem = await _stockItems.FirstOrDefaultAsync(
            item => item.ProductId == productId && item.WarehouseId == request.WarehouseId,
            cancellationToken);

        if (stockItem is null)
        {
            stockItem = new StockItem(product.Id, request.WarehouseId, 0);
            _stockItems.Add(stockItem);
        }

        stockItem.Adjust(request.QuantityDelta);
        _stockMovements.Add(new StockMovement(productId, request.WarehouseId, request.QuantityDelta, StockMovementType.Adjustment, request.Reason));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
