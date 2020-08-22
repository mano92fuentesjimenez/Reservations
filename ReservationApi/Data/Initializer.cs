using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using WebApplication.Models;

namespace WebApplication.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ReservationContext context, IWebHostEnvironment env)
        {
            context.Database.EnsureCreated();

            if (context.Clients.Any())
            {
                return;
            }

            var clients = new Client[]{
                new Client()
                { 
                    Name = "Manuel", BirthDate = DateTime.Parse("1992-05-15"), ContactType = ContactType.A
                },
            };
            foreach (Client s in clients)
            {
                context.Clients.Add(s);
            }
            context.SaveChanges();

            var manuelId = clients[0].ClientId;
            
            var rand = new Random();

            var reservations = new Reservation[]{
                new Reservation()
                {
                    ClientId = manuelId, Ranking = rand.Next(0, 6), Favorite = rand.Next(2) == 1, Name = "Second Dock", CreationTime = DateTime.Now, Description = "Description text"
                },
                new Reservation()
                {
                    ClientId = manuelId, Ranking = rand.Next(0, 6), Favorite = rand.Next(2) == 1, Name = "Primer Puerto", CreationTime = DateTime.Now, Description = "Description text"
                },
                new Reservation()
                {
                    ClientId = manuelId, Ranking = rand.Next(0, 6), Favorite = rand.Next(2) == 1, Name = "Stella", CreationTime = DateTime.Now, Description = "Description text"
                },
                new Reservation()
                {
                    ClientId = manuelId, Ranking = rand.Next(0, 6), Favorite = rand.Next(2) == 1, Name = "Island Creek", CreationTime = DateTime.Now, Description = "Description text"
                },
                new Reservation()
                {
                    ClientId = manuelId, Ranking = rand.Next(0, 6), Favorite = rand.Next(2) == 1, Name = "Second Dock", CreationTime = DateTime.Now, Description = "Description text"
                }
            };
            foreach (Reservation r in reservations)
            {
                context.Reservations.Add(r);
            }

            context.SaveChanges();

        }

    }
}