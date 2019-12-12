using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Models.Orders;

namespace HotelBooking.Services.Interfaces.Handlers
{
    internal interface IRoomAvailabilityHandler
    {
        Task<bool> IsRoomAvailableAsync(AddOrderModel addOrderModel);

        Task<bool> AreBookingDatesValid(AddOrderModel addOrderModel);
    }
}