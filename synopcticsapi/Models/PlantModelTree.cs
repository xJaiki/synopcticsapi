using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace synopcticsapi.Models {
    /// <summary>
    /// Database model for equipment in plant hierarchy
    /// </summary>
    public class PlantModelTree {
        public int? AreaId { get; set; }
        public int? Level { get; set; }

        [Key]
        [Required]
        public int EquipmentId { get; set; }

        public int? Parent { get; set; }

        [MaxLength(255)]
        public string EquipmentDescription { get; set; }

        [MaxLength(1024)]
        public string EquipmentLongDescription { get; set; }

        [MaxLength(1024)]
        public string EquipmentPathDescription { get; set; }

        [MaxLength(50)]
        public string SAPCode { get; set; }

        public bool? SAPEnabled { get; set; }
        public bool? IsManual { get; set; }
        public bool? IsSettable { get; set; }

        public int? DisplayOrder { get; set; }
        public int? LogicalOrder { get; set; }
        public int? ParallelLogicalOrder { get; set; }

        public bool? FlgMonitor { get; set; }

        [MaxLength(15)]
        public string IPAddress { get; set; }

        [MaxLength(50)]
        public string LastUpdate { get; set; }
    }
    /// <summary>
    /// DTO for hierarchical equipment data
    /// </summary>
    public class EquipmentDto {
        public string EquipmentId { get; set; }
        public string EquipmentDescription { get; set; }
        public string EquipmentLongDescription { get; set; }
        public string EquipmentPathDescription { get; set; }
        public string Level { get; set; }
        public string ParentId { get; set; }
        public string ObjectTypeId { get; set; }
        public string EquipmentPath { get; set; }
        public List<EquipmentDto> Children { get; set; } = new List<EquipmentDto>();
    }

    /// <summary>
    /// Response DTO for plant model tree
    /// </summary>
    public class PlantModelTreeResponse {
        public List<EquipmentTreeDto> EquipmentList { get; set; } = new List<EquipmentTreeDto>();
        public List<ErrorItemDto> ErrorList { get; set; } = new List<ErrorItemDto>();
    }

    /// <summary>
    /// Equipment tree wrapper for the top level structure
    /// </summary>
    public class EquipmentTreeDto {
        public List<EquipmentDto> Children { get; set; } = new List<EquipmentDto>();
    }

    /// <summary>
    /// Error item for response
    /// </summary>
    public class ErrorItemDto {
        public string Description { get; set; }
        public int Id { get; set; }

        public ErrorItemDto(string description, int id = 0)
        {
            Description = description;
            Id = id;
        }
    }
}