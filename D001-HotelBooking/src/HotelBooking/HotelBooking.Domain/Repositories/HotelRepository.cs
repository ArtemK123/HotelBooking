using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Search;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Domain.PipelinePattern;
using HotelBooking.Domain.PipelinePattern.Filters;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain.Repositories
{
    internal class HotelRepository : IHotelRepository
    {
        private readonly IApplicationDbContext context;

        public HotelRepository(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyCollection<HotelData>> GetAsync() => Map(await context.Hotels.ToListAsync());

        public async Task<HotelData> GetAsync(int id)
        {
            Hotel hotel = await context.Hotels.FindAsync(id);
            return hotel == null ? null : Map(hotel);
        }

        public IReadOnlyCollection<HotelData> GetByPage(int page, int pageSize) => Map(
            context
                .Hotels
                .OrderBy(hotel => hotel.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList());

        public IReadOnlyCollection<HotelData> GetSearchResultByPage(DataForSearch dataForSearch, int pageSize) => Map(GetSearchResult(dataForSearch)
                .Skip((dataForSearch.Page - 1) * pageSize).Take(pageSize).ToList());

        public int CountSearchResults(DataForSearch dataForSearch) => GetSearchResult(dataForSearch).Count();

        private IReadOnlyCollection<Hotel> GetSearchResult(DataForSearch dataForSearch)
        {
            HotelSelectionPipeline hotelSelectionPipeline = new HotelSelectionPipeline();
            hotelSelectionPipeline
                .Register(new HotelNameFilter(dataForSearch.HotelRequirements.Name))
                .Register(new HotelCityFilter(dataForSearch.HotelRequirements.City))
                .Register(new HotelsRoomFilter(dataForSearch.RoomRequirements, context.Rooms.ToList(), context.Orders.ToList()));
            return hotelSelectionPipeline.Process(context.Hotels.ToList());
        }

        private IReadOnlyCollection<HotelData> Map(IReadOnlyCollection<Hotel> hotels)
            => hotels.Select(Map).ToList();

        private HotelData Map(Hotel hotel)
            => new HotelData
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Images = Map(context.HotelImages.Where(image => image.HotelId == hotel.Id).ToList()),
                ManagerId = hotel.ManagerId,
                Address = hotel.Address,
                Description = hotel.Description,
                City = hotel.City,
                Rating = hotel.Rating
            };
        private IReadOnlyCollection<string> Map(IReadOnlyCollection<HotelImage> hotelImages)
            => hotelImages.Select(image => image.Path).ToList();
    }
}