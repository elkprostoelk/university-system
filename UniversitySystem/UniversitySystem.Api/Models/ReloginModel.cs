using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Api.Models
{
    public class ReloginModel
    {
        [Required]
        public int ReloginRole { get; set; }
    }
}