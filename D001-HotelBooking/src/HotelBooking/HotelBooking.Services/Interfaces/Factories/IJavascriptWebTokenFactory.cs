namespace HotelBooking.Services.Interfaces.Factories
{
    public interface IJavascriptWebTokenFactory
    {
        string Create(int userId);
    }
}