using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ComuneOnline.Models.DTOs
{
    public class CittadinoDetailsDTO
    {
        
        public int CittadinoId { get; set; }
        [Required]
        public required string Nome { get; set; }
        [Required]
        public required string Cognome { get; set; }
        public string? Email { get; set; }

        public string? Telefono { get; set; }
    }
}
