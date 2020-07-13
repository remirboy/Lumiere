using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Lumiere.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<ReservedSeat> ReservedSeats { get; set; }

        public User()
        {
            ReservedSeats = new List<ReservedSeat>();
        }
    }
}
