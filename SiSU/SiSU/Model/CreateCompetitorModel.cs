using System.ComponentModel.DataAnnotations;

namespace SiSU.Model
{
    public class CreateCompetitorModel
    {
        [Required]
        public string Name { get; set; }
    }
}
