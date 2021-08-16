using System.ComponentModel.DataAnnotations;

namespace ExploreCalifornia.DTOs
{
    public class AuthorizeRequestDto
    {
        public string AppToken { get; set; }

        public string AppSecret { get; set; }
    }
}