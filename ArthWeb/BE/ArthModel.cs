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
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<ItemSize> ItemSizes { get; set; }
        public virtual DbSet<ItemQuantity> ItemQuantities { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }

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

            modelBuilder.Entity<Item>()
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

            modelBuilder.Entity<Address>()
                .Property(e => e.AddressDetail)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.AltPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Landmark)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Pin)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.EmailID)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.ItemKey)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.ItemSubType)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.ItemType)
                .IsUnicode(false);
        }
    }
}
