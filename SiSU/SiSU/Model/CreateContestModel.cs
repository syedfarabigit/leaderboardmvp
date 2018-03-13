using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiSU.Model
{
    public class CreateContestModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int TournamentId { get; set; }
        [Required]
        public List<int> CompetitorIds { get; set; }
    }
}
