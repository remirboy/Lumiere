using System;

namespace Lumiere.Models
{
    public class FilmFeedback
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
