using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication.Models
{
    public enum ContactType
    {
        A, B,
    }
    
    public class Client
    {
        public int ClientId { get; set; }
        
        public string Name { get; set; }
        public ContactType ContactType { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}