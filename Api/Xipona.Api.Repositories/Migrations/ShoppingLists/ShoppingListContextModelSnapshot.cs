﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Contexts;

#nullable disable

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.ShoppingLists
{
    [DbContext(typeof(ShoppingListContext))]
    partial class ShoppingListContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("DiscountPrice")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ItemTypeId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ShoppingListId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ItemTypeId");

                    b.HasIndex("ShoppingListId");

                    b.ToTable("Discount");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities.ItemsOnList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("InBasket")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ItemTypeId")
                        .HasColumnType("char(36)");

                    b.Property<float>("Quantity")
                        .HasColumnType("float");

                    b.Property<Guid>("SectionId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ShoppingListId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingListId");

                    b.ToTable("ItemsOnLists");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities.ShoppingList", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset?>("CompletionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("ShoppingLists");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities.Discount", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities.ShoppingList", "ShoppingList")
                        .WithMany("Discounts")
                        .HasForeignKey("ShoppingListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShoppingList");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities.ItemsOnList", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities.ShoppingList", "ShoppingList")
                        .WithMany("ItemsOnList")
                        .HasForeignKey("ShoppingListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShoppingList");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities.ShoppingList", b =>
                {
                    b.Navigation("Discounts");

                    b.Navigation("ItemsOnList");
                });
#pragma warning restore 612, 618
        }
    }
}
