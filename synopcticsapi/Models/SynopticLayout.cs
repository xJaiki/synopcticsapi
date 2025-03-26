using System;
using System.ComponentModel.DataAnnotations;

namespace synopcticsapi.Models {
    /// <summary>
    /// Represents a synoptic layout from the SynopticLayouts table
    /// </summary>
    public class SynopticLayout {
        /// <summary>
        /// The layout identifier (primary key)
        /// </summary>
        [Key]
        [Required]
        [StringLength(50)]
        public string Layout { get; set; }

        /// <summary>
        /// The area identifier
        /// </summary>
        [StringLength(50)]
        public string AreaId { get; set; }

        /// <summary>
        /// The SVG content of the synoptic
        /// </summary>
        public string Svg { get; set; }

        /// <summary>
        /// Backup of the SVG content
        /// </summary>
        public string SvgBackup { get; set; }
    }
}