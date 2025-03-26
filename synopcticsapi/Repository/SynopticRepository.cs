using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using synopcticsapi.Data;
using synopcticsapi.Models;

namespace synopcticsapi.Repository {
    /// <summary>
    /// Implementation of the synoptic repository
    /// </summary>
    public class SynopticRepository : ISynopticRepository {
        private readonly SynopticDbContext _context;

        public SynopticRepository(SynopticDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all synoptic layouts
        /// </summary>
        /// <returns>A list of synoptic layouts</returns>
        public async Task<List<SynopticLayout>> GetSynopticListAsync()
        {
            return await _context.SynopticLayouts.ToListAsync();
        }

        /// <summary>
        /// Gets a specific synoptic layout by its identifier
        /// </summary>
        /// <param name="layout">The layout identifier</param>
        /// <returns>The synoptic layout or null if not found</returns>
        public async Task<SynopticLayout> GetSynopticAsync(string layout)
        {
            return await _context.SynopticLayouts.FindAsync(layout);
        }

        /// <summary>
        /// Inserts a new synoptic layout
        /// </summary>
        /// <param name="synopticLayout">The synoptic layout to insert</param>
        /// <returns>The result of the operation</returns>
        public async Task<bool> InsertSynopticAsync(SynopticLayout synopticLayout)
        {
            // Check if the synoptic already exists
            var existing = await _context.SynopticLayouts.FindAsync(synopticLayout.Layout);
            if (existing != null)
            {
                return false; // Synoptic already exists
            }

            _context.SynopticLayouts.Add(synopticLayout);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        /// Updates an existing synoptic layout
        /// </summary>
        /// <param name="synopticLayout">The synoptic layout to update</param>
        /// <returns>The result of the operation</returns>
        public async Task<bool> UpdateSynopticAsync(SynopticLayout synopticLayout)
        {
            // Find the existing synoptic
            var existing = await _context.SynopticLayouts.FindAsync(synopticLayout.Layout);
            if (existing == null)
            {
                return false; // Synoptic doesn't exist
            }

            // Update the properties
            existing.AreaId = synopticLayout.AreaId;
            existing.Svg = synopticLayout.Svg;
            existing.SvgBackup = synopticLayout.SvgBackup;

            // Or alternatively:
            // _context.Entry(existing).CurrentValues.SetValues(synopticLayout);

            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}