using System.Collections.Generic;
using System.Threading.Tasks;
using synopcticsapi.Models;
using static synopcticsapi.Repository.SynopticRepository;

namespace synopcticsapi.Repository {
    /// <summary>
    /// Interface for synoptic data operations
    /// </summary>
    public interface ISynopticRepository {
        /// <summary>
        /// Gets all synoptic layouts
        /// </summary>
        /// <returns>A list of synoptic layouts</returns>
        Task<List<SynopticLayout>> GetSynopticListAsync();

        /// <summary>
        /// Gets a specific synoptic layout by its identifier
        /// </summary>
        /// <param name="layout">The layout identifier</param>
        /// <returns>The synoptic layout or null if not found</returns>
        Task<SynopticLayout> GetSynopticAsync(string layout);

        /// <summary>
        /// Inserts a new synoptic layout
        /// </summary>
        /// <param name="synopticLayout">The synoptic layout to insert</param>
        /// <returns>The result of the operation</returns>
        Task<bool> InsertSynopticAsync(SynopticLayout synopticLayout);

        /// <summary>
        /// Updates an existing synoptic layout
        /// </summary>
        /// <param name="synopticLayout">The synoptic layout to update</param>
        /// <returns>The result of the operation</returns>
        Task<bool> UpdateSynopticAsync(SynopticLayout synopticLayout);


        /// <summary>
        /// Gets the current data for a specific synoptic
        /// </summary>
        /// <param name="layout">The layout identifier</param>
        /// <returns>The current synoptic data</returns>
        Task<List<SynopticDataItem>> GetSynopticDataAsync(string layout);

        Task<bool> UpdateSynopticElementDataAsync(
    string elementId,
    string synopticLayout,
    string text1,
    string text2,
    string text3,
    int status);
    }
}