using System.Threading.Tasks;

namespace HotelBooking.Contracts.Services.PipelinePattern.Interfaces
{
    public interface IFilter<T>
    {
        T Execute(T input);
    }
}