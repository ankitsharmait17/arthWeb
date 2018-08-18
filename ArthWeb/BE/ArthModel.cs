namespace BE
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ArthModel : DbContext
    {
        public ArthModel()
            : base("name=ArthModel1")
        {
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemMapping> ItemMappings { get; set; }
        public virtual DbSet<ItemSubType> ItemSubTypes { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .Property(e => e.ItemKey)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.DescriptionLong)
                .IsUnicode(false);

            modelBuilder.Entity<ItemMapping>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<ItemMapping>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.ItemMapping)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemSubType>()
                .Property(e => e.ItemSubTypeKey)
                .IsUnicode(false);

            modelBuilder.Entity<ItemSubType>()
                .HasMany(e => e.ItemMappings)
                .WithRequired(e => e.ItemSubType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemType>()
                .Property(e => e.ItemTypeKey)
                .IsUnicode(false);

            modelBuilder.Entity<ItemType>()
                .HasMany(e => e.ItemMappings)
                .WithRequired(e => e.ItemType)
                .WillCascadeOnDelete(false);
        }
    }
}
