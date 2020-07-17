using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.ViewModels
{
    [JsonObject]
    public class ReservedSeatViewModel
    {
        public int[] SeatNumbers { get; set; }
        public int RoomNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Price { get; set; }
        public Guid FilmId { get; set; }
    }
}
