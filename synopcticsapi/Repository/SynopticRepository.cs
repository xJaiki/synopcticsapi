using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        /// <summary>
        /// Model class for synoptic data items
        /// </summary>
        public class SynopticDataItem {
            public string Id { get; set; }
            public string Text1 { get; set; }
            public string Text2 { get; set; }
            public string Text3 { get; set; }
            public int Status { get; set; }
            public string LastUpdate { get; set; }
        }

        /// <summary>
        /// Gets the current data for a specific synoptic
        /// </summary>
        /// <param name="layout">The layout identifier</param>
        /// <returns>The current synoptic data</returns>
        public async Task<List<SynopticDataItem>> GetSynopticDataAsync(string layout)
        {
            // Prima ottieni i dati senza formattare la data
            var data = await _context.SynopticData
                .Where(d => d.SynopticLayout == layout)
                .Select(d => new {
                    d.ElementId,
                    d.Text1,
                    d.Text2,
                    d.Text3,
                    d.Status,
                    d.LastUpdate
                })
                .ToListAsync();

            // Poi formatta i dati dopo che sono stati recuperati dal database
            return data.Select(d => new SynopticDataItem
            {
                Id = d.ElementId,
                Text1 = d.Text1,
                Text2 = d.Text2,
                Text3 = d.Text3,
                Status = d.Status,
                LastUpdate = d.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();
        }
        public async Task<bool> UpdateSynopticElementDataAsync(
            string elementId,
            string synopticLayout,
            string text1,
            string text2,
            string text3,
            int status)
        {
            var data = await _context.SynopticData
                .FirstOrDefaultAsync(d => d.ElementId == elementId && d.SynopticLayout == synopticLayout);

            if (data == null)
            {
                // L'elemento non esiste, creane uno nuovo
                data = new SynopticData
                {
                    ElementId = elementId,
                    SynopticLayout = synopticLayout
                };
                _context.SynopticData.Add(data);
            }

            // Aggiorna i campi
            data.Text1 = text1;
            data.Text2 = text2;
            data.Text3 = text3;
            data.Status = status;
            data.LastUpdate = DateTime.Now;

            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}