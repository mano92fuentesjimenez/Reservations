using System;

namespace WebApplication.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }  
        public bool Favorite { get; set; }
        public DateTime CreationTime { get; set; }
        public int Ranking { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}