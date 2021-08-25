using System;
using System.ComponentModel.DataAnnotations;


namespace ExploreCalifornia.Data.Entities
{
    // Sample tuple:
    // INSERT INTO [ExploreCalifornia].[dbo].[AuthorizedApps]
    // (Name, AppToken, AppSecret, TokenExpiration)
    // VALUES
    // ('SocaClient', 'reallyreallylongapplicationtoken', 'reallyreallylongsupersecrettoken', '2021-08-25 00:00:00.000');
    // AuthorizedClient/App
    public class AuthorizedApp
    {
        public int AuthorizedAppId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(32)]
        public string AppToken { get; set; }

        [Required]
        [StringLength(32)]
        public string AppSecret { get; set; }

        public DateTime TokenExpiration { get; set; }
    }
}
