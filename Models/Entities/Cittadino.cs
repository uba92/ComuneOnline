using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComuneOnline.Models.Entities
{
    [Table("cittadini")]
    public class Cittadino
    {
        [Key]
        public int CittadinoId { get; set; }

        [Required]
        public required string Nome { get; set; }
        [Required]
        public required string Cognome { get; set; }
        [Required]
        public DateTime DataNascita { get; set; }
        [Required]
        public string? LuogoNascita { get; set; }
        [Required]
        public string? IndirizzoResidenza { get; set; }

        public string? Telefono { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
    }
}
