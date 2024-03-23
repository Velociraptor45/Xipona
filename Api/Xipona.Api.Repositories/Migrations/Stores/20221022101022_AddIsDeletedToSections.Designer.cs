﻿// <auto-generated />

#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.Stores
{
    [DbContext(typeof(StoreContext))]
    [Migration("20221022101022_AddIsDeletedToSections")]
    partial class AddIsDeletedToSections
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Section", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDefaultSection")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SortIndex")
                        .HasColumnType("int");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Section", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Store", "Store")
                        .WithMany("Sections")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Store", b =>
                {
                    b.Navigation("Sections");
                });
#pragma warning restore 612, 618
        }
    }
}