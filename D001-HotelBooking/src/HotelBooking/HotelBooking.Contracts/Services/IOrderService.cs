using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Models;
using HotelBooking.Contracts.Dto.Models.Orders;

namespace HotelBooking.Contracts.Services
{
    public interface IOrderService
    {
        Task<IReadOnlyCollection<GetOrderModel>> GetAsync();

        Task<GetOrderModel> GetAsync(int id);

        Task<IReadOnlyCollection<GetOrderModel>> GetByClientAsync(int id);

        Task<IReadOnlyCollection<GetOrderModel>> GetByHotelAsync(int hotelId);

        Task<DefaultResponseModel> AddAsync(AddOrderModel orderModel);
    }
}