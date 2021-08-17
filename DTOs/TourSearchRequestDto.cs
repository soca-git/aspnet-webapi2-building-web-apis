namespace ExploreCalifornia.DTOs
{
    // DTO -> Data Transfer Object.
    public class TourSearchRequestDto
    {
        // Each property in the DTO is set to a default value.
        // This allows the query to run without a DTO specified in the request payload.
        public decimal MinPrice { get; set; } = decimal.MinValue;
        public decimal MaxPrice { get; set; } = decimal.MaxValue;
    }
}