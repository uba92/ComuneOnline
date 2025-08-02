using System.ComponentModel.DataAnnotations;

namespace ComuneOnline.Models.Entities
{
    public class Cittadino
    {
        [Key]
        public int CittadinoId { get; set; }

        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cognome { get; set; }
        [Required]
        public DateTime DataNascita { get; set; }
        [Required]
        public string LuogoNascita { get; set; }
        [Required]
        public string IndirizzoResidenza { get; set; }

        public string? Telefono { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
    }
}
