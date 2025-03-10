﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;

#nullable disable

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.Items
{
    [DbContext(typeof(ItemContext))]
    partial class ItemContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Items.Entities.AvailableAt", b =>
                {
                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)")
                        .HasColumnOrder(1);

                    b.Property<Guid>("StoreId")
                        .HasColumnType("char(36)")
                        .HasColumnOrder(2);

                    b.Property<Guid>("DefaultSectionId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("ItemId", "StoreId");

                    b.ToTable("AvailableAts");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("CreatedFrom")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsTemporary")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("ItemCategoryId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ManufacturerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("PredecessorId")
                        .HasColumnType("char(36)");

                    b.Property<float?>("QuantityInPacket")
                        .HasColumnType("float");

                    b.Property<int>("QuantityType")
                        .HasColumnType("int");

                    b.Property<int?>("QuantityTypeInPacket")
                        .HasColumnType("int");

                    b.Property<DateTime>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("PredecessorId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("PredecessorId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("PredecessorId");

                    b.ToTable("ItemTypes");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemTypeAvailableAt", b =>
                {
                    b.Property<Guid>("ItemTypeId")
                        .HasColumnType("char(36)")
                        .HasColumnOrder(1);

                    b.Property<Guid>("StoreId")
                        .HasColumnType("char(36)")
                        .HasColumnOrder(2);

                    b.Property<Guid>("DefaultSectionId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("ItemTypeId", "StoreId");

                    b.ToTable("ItemTypeAvailableAts");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Items.Entities.AvailableAt", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item", "Item")
                        .WithMany("AvailableAt")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item", "Predecessor")
                        .WithMany()
                        .HasForeignKey("PredecessorId");

                    b.Navigation("Predecessor");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item", "Item")
                        .WithMany("ItemTypes")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType", "Predecessor")
                        .WithMany()
                        .HasForeignKey("PredecessorId");

                    b.Navigation("Item");

                    b.Navigation("Predecessor");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemTypeAvailableAt", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType", "ItemType")
                        .WithMany("AvailableAt")
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemType");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item", b =>
                {
                    b.Navigation("AvailableAt");

                    b.Navigation("ItemTypes");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType", b =>
                {
                    b.Navigation("AvailableAt");
                });
#pragma warning restore 612, 618
        }
    }
}
