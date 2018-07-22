namespace BE
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=ArthModel")
        {
        }

        public virtual DbSet<ItemMapping> ItemMappings { get; set; }
        public virtual DbSet<ItemSubType> ItemSubTypes { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemMapping>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<ItemSubType>()
                .Property(e => e.ItemSubType1)
                .IsUnicode(false);

            modelBuilder.Entity<ItemSubType>()
                .HasMany(e => e.ItemMappings)
                .WithRequired(e => e.ItemSubType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemType>()
                .Property(e => e.ItemType1)
                .IsUnicode(false);

            modelBuilder.Entity<ItemType>()
                .HasMany(e => e.ItemMappings)
                .WithRequired(e => e.ItemType)
                .WillCascadeOnDelete(false);
        }
    }
}
