using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using synopcticsapi.Data;
using synopcticsapi.Models;

namespace synopcticsapi.Repository
{
    /// <summary>
    /// Interface for plant model repository operations
    /// </summary>
    public interface IPlantModelTreeRepository
    {
        /// <summary>
        /// Gets the hierarchical plant model tree
        /// </summary>
        Task<List<EquipmentDto>> GetPlantModelTreeTreeAsync();
    }
}