using System.ComponentModel.DataAnnotations;

namespace ExploreCalifornia.Data.Entities
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public int TourId { get; set; }

        [Required]
        [StringLength(200)]
        public string CustomerFullName { get; set; }

        [Required]
        [StringLength(15)]
        public string CustomerPhone { get; set; }

        // Tour navigation property.
        public Tour Tour { get; set; }
    }
}
