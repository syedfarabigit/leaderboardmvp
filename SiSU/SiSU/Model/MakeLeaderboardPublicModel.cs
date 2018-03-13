using System.ComponentModel.DataAnnotations;

namespace SiSU.Model
{
    public class MakeLeaderboardPublicModel
    {
        [Required]
        public int ContestId { get; set; }
    }
}
