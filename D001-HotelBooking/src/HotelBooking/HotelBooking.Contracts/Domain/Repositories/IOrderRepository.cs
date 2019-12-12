using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Orders;

namespace HotelBooking.Contracts.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<IReadOnlyCollection<GetOrderModel>> GetAsync();

        Task<GetOrderModel> GetAsync(int id);

        Task<IReadOnlyCollection<GetOrderModel>> GetByHotelAsync(int id);

        Task<IReadOnlyCollection<GetOrderModel>> GetByClientAsync(int id);

        Task<OrderData> AddAsync(OrderData orderData);
    }
}