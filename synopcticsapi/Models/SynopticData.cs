using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace synopcticsapi.Models {
    /// <summary>
    /// Represents synoptic element data from the SynopticData table
    /// </summary>
    public class SynopticData {
        [Key]
        [Required]
        [StringLength(50)]
        public string ElementId { get; set; }

        [Required]
        [StringLength(50)]
        public string SynopticLayout { get; set; }

        public string Text1 { get; set; }

        public string Text2 { get; set; }

        public string Text3 { get; set; }

        public int Status { get; set; }

        public DateTime LastUpdate { get; set; }

        // Relazione con SynopticLayout
        [ForeignKey("SynopticLayout")]
        public virtual SynopticLayout Layout { get; set; }
    }
}