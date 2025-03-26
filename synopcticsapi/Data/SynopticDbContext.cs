using System.Collections.Generic;
using System.Data.Entity;
using synopcticsapi.Models;

namespace synopcticsapi.Data {
    /// <summary>
    /// Database context for synoptic operations
    /// </summary>
    public class SynopticDbContext : DbContext {
        public SynopticDbContext() : base("name=SynopticDbConnection")
        {
        }

        /// <summary>
        /// DbSet for SynopticLayouts table
        /// </summary>
        public DbSet<SynopticLayout> SynopticLayouts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure the primary key for the SynopticLayout
            modelBuilder.Entity<SynopticLayout>()
                .HasKey(s => s.Layout);

            // Configure the table name
            modelBuilder.Entity<SynopticLayout>()
                .ToTable("SynopticLayouts");

            base.OnModelCreating(modelBuilder);
        }
    }
}