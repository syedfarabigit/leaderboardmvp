using SiSU.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace SiSU.Model
{
    public class ContestResultModel
    {
        [Required]
        public int ContestId { get; set; }
        public int? WinnerId { get; set; }
        [Required]
        public bool IsDrawn { get; set; }
    }
}
