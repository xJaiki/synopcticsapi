using synopcticsapi.Data;
using synopcticsapi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synopcticsapi.Repository {

    /// <summary>
    /// Repository implementation for plant model operations using Entity Framework
    /// </summary>
    public class PlantModelTreeRepository : IPlantModelTreeRepository {
        private readonly SynopticDbContext _context;

        public PlantModelTreeRepository(SynopticDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the hierarchical plant model tree using Entity Framework
        /// </summary>
        public async Task<List<EquipmentDto>> GetPlantModelTreeTreeAsync()
        {
            // Fetch all plant model data
            var PlantModelTreeData = await _context.PlantModels
                .OrderBy(p => p.Level)
                .ThenBy(p => p.LogicalOrder)
                .ToListAsync();

            // Convert to DTO and build hierarchy
            return BuildEquipmentHierarchy(PlantModelTreeData);
        }


        private List<EquipmentDto> BuildEquipmentHierarchy(List<PlantModelTree> flatData)
        {
            // Group by AreaId first (should typically be just one area with ID 2)
            var areas = flatData.Where(p => p.Level == 2).ToList();
            var result = new List<EquipmentDto>();

            foreach (var area in areas)
            {
                var areaDto = MapToEquipmentDto(area);
                result.Add(areaDto);

                // Find all line groups (children of this area)
                var lineGroups = flatData.Where(p => p.Level == 3 && p.Parent == area.EquipmentId).ToList();
                foreach (var lineGroup in lineGroups)
                {
                    var lineGroupDto = MapToEquipmentDto(lineGroup);
                    areaDto.Children.Add(lineGroupDto);

                    // Find all lines (children of this line group)
                    var lines = flatData.Where(p => p.Level == 4 && p.Parent == lineGroup.EquipmentId).ToList();
                    foreach (var line in lines)
                    {
                        var lineDto = MapToEquipmentDto(line);
                        lineDto.ObjectTypeId = ExtractObjectTypeId(lineDto.EquipmentDescription);
                        lineGroupDto.Children.Add(lineDto);

                        // Find all equipment (children of this line)
                        var equipment = flatData.Where(p => p.Level == 5 && p.Parent == line.EquipmentId).ToList();
                        foreach (var eq in equipment)
                        {
                            var eqDto = MapToEquipmentDto(eq);
                            eqDto.ObjectTypeId = ExtractObjectTypeId(eqDto.EquipmentDescription);
                            eqDto.EquipmentPath = BuildEquipmentPath(area.EquipmentId, lineGroup.EquipmentId, line.EquipmentId, eq.EquipmentId);
                            lineDto.Children.Add(eqDto);

                            // Find all level 6 components (children of this equipment)
                            var components = flatData.Where(p => p.Level == 6 && p.Parent == eq.EquipmentId).ToList();
                            foreach (var comp in components)
                            {
                                var compDto = MapToEquipmentDto(comp);
                                compDto.ObjectTypeId = ExtractObjectTypeId(compDto.EquipmentDescription);
                                compDto.EquipmentPath = BuildEquipmentPath(area.EquipmentId, lineGroup.EquipmentId, line.EquipmentId, eq.EquipmentId, comp.EquipmentId);
                                eqDto.Children.Add(compDto);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Builds equipment path string in the format needed by frontend
        /// </summary>
        private string BuildEquipmentPath(int areaId, int lineGroupId, int lineId, int equipmentId, int? componentId = null)
        {
            if (componentId.HasValue)
                return $"{areaId}-{lineGroupId}-{lineId}-{equipmentId}-{componentId}";
            else
                return $"{areaId}-{lineGroupId}-{lineId}-{equipmentId}";
        }

        /// <summary>
        /// Maps a database model to DTO
        /// </summary>
        private EquipmentDto MapToEquipmentDto(PlantModelTree model)
        {
            return new EquipmentDto
            {
                EquipmentId = model.EquipmentId.ToString(),
                EquipmentDescription = model.EquipmentDescription,
                EquipmentLongDescription = model.EquipmentLongDescription,
                EquipmentPathDescription = model.EquipmentPathDescription,
                Level = model.Level.ToString(),
                ParentId = model.Parent?.ToString(),
                Children = new List<EquipmentDto>()
            };
        }

        /// <summary>
        /// Extracts object type ID from equipment description
        /// Based on the pattern seen in the data (e.g., FWA)
        /// </summary>
        private string ExtractObjectTypeId(string description)
        {
            if (string.IsNullOrEmpty(description)) return string.Empty;

            // Extract prefix like FWA, WTI, etc.
            var parts = description.Split('_');
            if (parts.Length > 0)
            {
                var prefix = parts[0];
                if (prefix.Contains("FWA") || prefix.Contains("WTI"))
                {
                    return prefix;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Builds equipment path string in the format needed by frontend
        /// </summary>
        private string BuildEquipmentPath(int areaId, int lineGroupId, int lineId, int equipmentId)
        {
            return $"{areaId}-{lineGroupId}-{lineId}-{equipmentId}";
        }
    }

}
