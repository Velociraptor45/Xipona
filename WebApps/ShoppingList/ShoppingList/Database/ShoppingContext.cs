using System;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Database.Entities;

namespace ShoppingList.Database
{
    public partial class ShoppingContext : DbContext
    {
        public ShoppingContext()
        {
        }

        public ShoppingContext(DbContextOptions<ShoppingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemOnShoppingList> ItemOnShoppingList { get; set; }
        public virtual DbSet<QuantityType> QuantityType { get; set; }
        public virtual DbSet<Entities.ShoppingList> ShoppingList { get; set; }
        public virtual DbSet<Store> Store { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasIndex(e => e.QuantityTypeId)
                    .HasName("QuantityTypeId");

                entity.Property(e => e.ItemId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PricePerQuantity)
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("'1.00'");

                entity.Property(e => e.QuantityTypeId).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.QuantityType)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.QuantityTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Item_ibfk_1");
            });

            modelBuilder.Entity<ItemOnShoppingList>(entity =>
            {
                entity.HasIndex(e => e.ItemId)
                    .HasName("ItemId");

                entity.HasIndex(e => e.ShoppingListId)
                    .HasName("ShoppingListId");

                entity.Property(e => e.ItemOnShoppingListId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.ItemId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Quantity).HasColumnType("int(10) unsigned");

                entity.Property(e => e.ShoppingListId).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemOnShoppingList)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ItemOnShoppingList_ibfk_2");

                entity.HasOne(d => d.ShoppingList)
                    .WithMany(p => p.ItemOnShoppingList)
                    .HasForeignKey(d => d.ShoppingListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ItemOnShoppingList_ibfk_1");
            });

            modelBuilder.Entity<QuantityType>(entity =>
            {
                entity.Property(e => e.QuantityTypeId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Entities.ShoppingList>(entity =>
            {
                entity.HasIndex(e => e.StoreId)
                    .HasName("StoreId");

                entity.Property(e => e.ShoppingListId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.CompletionDate).HasColumnType("date");

                entity.Property(e => e.StoreId).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ShoppingList)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ShoppingList_ibfk_1");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.StoreId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
