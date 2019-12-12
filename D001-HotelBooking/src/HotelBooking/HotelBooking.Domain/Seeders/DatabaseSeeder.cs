using System;
using System.Collections.Generic;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Domain.Seeders;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Entities.Users;
using HotelBooking.Domain.Interfaces;

namespace HotelBooking.Domain
{
    internal class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IApplicationDbContext context;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IApplicationRoleRepository applicationRoleRepository;

        private const string Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
            "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
            "quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute " +
            "irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
            "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim " +
            "id est laborum.";

        public DatabaseSeeder(
            IApplicationDbContext context,
            IApplicationUserRepository applicationUserRepository,
            IApplicationRoleRepository applicationRoleRepository)
        {
            this.context = context;
            this.applicationUserRepository = applicationUserRepository;
            this.applicationRoleRepository = applicationRoleRepository;
        }

        public void Seed()
        {
            var clientRole = new RoleData { Name = "Client" };
            var agencyRole = new RoleData { Name = "Agency" };

            var creditCard1 = new CreditCard
            {
                Number = 1234567887654321,
                CV2 = 123,
                DueDate = "02/12",
            };

            var creditCard2 = new CreditCard
            {
                Number = 3211234322341122,
                CV2 = 122,
                DueDate = "06/7"
            };

            var creditCard3 = new CreditCard
            {
                Number = 3211234322341189,
                CV2 = 123,
                DueDate = "02/2"
            };

            var user1 = new UserData
            {
                Email = "TestEmail1@gmail.com",
                Password = "Qwerty",
                RoleId = 1,
            };

            var client1 = new Client
            {
                Name = "Tester1",
                Surname = "TesterSur1",
                Passport = "MT23456713",
                PhotoPath = "default/profile.png",
                Telephone = "+38097123456",
                UserId = 1,
            };

            var user2 = new UserData
            {
                Email = "TestEmail2@gmail.com",
                Password = "Qwerty311@ewq",
                RoleId = 1,
            };

            var client2 = new Client
            {
                Name = "Tester2",
                Surname = "TesterSur2",
                Passport = "MT22222222",
                PhotoPath = "default/profile.png",
                Telephone = "+38097123456",
                UserId = 2,
            };

            var user3 = new UserData
            {
                Email = "TestEmail3@gmail.com",
                Password = "Qwerty555@5",
                RoleId = 1,
            };

            var client3 = new Client
            {
                Name = "Tester3",
                Surname = "TesterSur3",
                Passport = "MT23456713",
                PhotoPath = "default/profile.png",
                Telephone = "+38097123456",
                UserId = 3,
            };

            var managers = new[]
            {
                new Manager
                {
                    UserId = 4
                },
                new Manager
                {
                    UserId = 5
                },
                new Manager
                {
                    UserId = 6
                }
            };

            context.Managers.AddRange(managers);

            AddHotels();
            AddHotelImages();
            AddRooms();
            AddRoomImages();
            
            // AddOrders();

            context.Clients.Add(client1);
            context.Clients.Add(client2);
            context.Clients.Add(client3);
            context.SaveChanges();
        }

        private void AddHotels()
        {
            var hotels = new List<Hotel>();

            hotels.Add(new Hotel
            {
                Name = "Burj Al Arab",
                Address = "Jumeirah St",
                City = "Dubai",
                Description = Description,
                Rating = 5,
                ManagerId = 1
            });

            hotels.Add(new Hotel
            {
                Name = "The Plaza Hotel",
                Address = "768 5th Ave",
                City = "New York, NY",
                Description = Description,
                Rating = 5,
                ManagerId = 2
            });

            hotels.Add(new Hotel
            {
                Name = "Marina Bay Sands",
                Address = "10 Bayfront Avenue",
                City = "Singapore",
                Description = Description,
                Rating = 4,
                ManagerId = 3
            });

            hotels.Add(new Hotel
            {
                Name = "Borgo Egnazia",
                Address = "Strada Comunale Egnazia, 72015",
                City = "Savelletri di Fasano",
                Description = Description,
                Rating = 4,
                ManagerId = 2
            });

            foreach (var hotel in hotels)
            {
                context.Hotels.Add(hotel);
            }
        }

        private void AddRooms()
        {
            var rooms = new List<Room>();

            rooms.Add(new Room
            {
               Description = Description,
               HotelId = 1,
               NumberOfPeople = 2,
               PricePerNight = 450,
               Square = 60
            });

            rooms.Add(new Room
            {
               Description = Description,
               HotelId = 1,
               NumberOfPeople = 3,
               PricePerNight = 550,
               Square = 65
            });

            rooms.Add(new Room
            {
                Description = Description,
                HotelId = 2,
                NumberOfPeople = 2,
                PricePerNight = 700,
                Square = 80
            });

            rooms.Add(new Room
            {
                Description = Description,
                HotelId = 3,
                NumberOfPeople = 4,
                PricePerNight = 680,
                Square = 95
            });

            rooms.Add(new Room
            {
                Description = Description,
                HotelId = 4,
                NumberOfPeople = 2,
                PricePerNight = 390,
                Square = 55
            });

            foreach (var room in rooms)
            {
                context.Rooms.Add(room);
            }
        }

        private void AddHotelImages()
        {
            var images = new List<HotelImage>();
            images.Add(new HotelImage
            {
                HotelId = 1,
                Path = "/Hotels/1/1.jpg"
            });
            images.Add(new HotelImage
            {
                HotelId = 2,
                Path = "/Hotels/2/1.jpg"
            });
            images.Add(new HotelImage
            {
                HotelId = 3,
                Path = "/Hotels/3/1.jpg"
            });
            images.Add(new HotelImage
            {
                HotelId = 4,
                Path = "/Hotels/4/1.jpg"
            });

            foreach (var image in images)
            {
                context.HotelImages.Add(image);
            }
        }

        private void AddRoomImages()
        {
            var images = new List<RoomImage>();
            images.Add(new RoomImage
            {
                RoomId = 1,
                Path = "/Rooms/1/1.jpg"
            });
            images.Add(new RoomImage
            {
                RoomId = 2,
                Path = "/Rooms/2/1.jpg"
            });
            images.Add(new RoomImage
            {
                RoomId = 3,
                Path = "/Rooms/3/1.jpg"
            });
            images.Add(new RoomImage
            {
                RoomId = 4,
                Path = "/Rooms/4/1.jpg"
            });

            foreach (var image in images)
            {
                context.RoomImages.Add(image);
            }
        }

        private void AddOrders()
        {
            for (int i = 1; i < 4; i++)
            {
                context.Orders.Add(new Order()
                {
                    ClientId = 1,
                    RoomId = i,
                    Name = "Artem",
                    Surname = "Kuchma",
                    CheckIn = DateTime.UtcNow.AddDays(i),
                    CheckOut = DateTime.UtcNow.AddDays(5 + i)
                });
            }
        }
    }
}