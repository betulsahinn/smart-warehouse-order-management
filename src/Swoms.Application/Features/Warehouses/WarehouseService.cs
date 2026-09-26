using AutoMapper;
using Swoms.Application.Common.Interfaces;
using Swoms.Domain.Entities;
using Swoms.Domain.ValueObjects;

namespace Swoms.Application.Features.Warehouses;

public sealed class WarehouseService : IWarehouseService
{
    private readonly IRepository<Warehouse> _warehouses;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WarehouseService(IRepository<Warehouse> warehouses, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _warehouses = warehouses;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<WarehouseDto>> GetWarehousesAsync(CancellationToken cancellationToken = default)
    {
        var warehouses = await _warehouses.ListAsync(cancellationToken: cancellationToken);
        return _mapper.Map<IReadOnlyList<WarehouseDto>>(warehouses);
    }

    public async Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseRequest request, CancellationToken cancellationToken = default)
    {
        var warehouse = new Warehouse(
            request.Code,
            request.Name,
            new Address(request.Line1, request.Line2, request.City, request.State, request.PostalCode, request.Country));

        _warehouses.Add(warehouse);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WarehouseDto>(warehouse);
    }
}
