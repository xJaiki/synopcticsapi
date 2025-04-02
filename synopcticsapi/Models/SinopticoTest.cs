using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace synopcticsapi.Models
{

    public class SinopticoTest
    {
        [Key]
        public int SinopticoTestId { get; set; }

        [Required]
        [MaxLength(5)]
        public string Text1 { get; set; }

        [Required]
        [MaxLength(5)]
        public string Text2 { get; set; }

        [Required]
        [MaxLength(5)]
        public string Text3 { get; set; }

        public int Status { get; set; }

        [MaxLength(50)]
        public string LastUpdate { get; set; }
    }
}