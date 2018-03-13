using System.Collections.Generic;

namespace SiSU.Model
{
    public class Contest
    {
        public int ContestId { get; set; }
        public string Name { get; set; }
        public int TournamentId { get; set; }
        public List<Competitor> Competitors { get; set; }
    }
}
