// ----------------------------------------------------------------------
// <copyright file="FamilyDbContext.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace SoloX.TableModel.UTests.Samples
{
    public class FamilyDbContext : DbContext
    {
        public FamilyDbContext()
        {
        }

        public FamilyDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Family> Families { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Family>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Person>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Family>()
                .HasMany(e => e.Members)
                .WithOne(e => e.Family);

            modelBuilder.Entity<Person>()
                .HasOne(e => e.Family)
                .WithMany(e => e.Members);

            base.OnModelCreating(modelBuilder);
        }

    }
}
