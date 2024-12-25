﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Contexts;

#nullable disable

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.Recipes
{
    [DbContext(typeof(RecipeContext))]
    partial class RecipeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Ingredient", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<bool?>("AddToShoppingListByDefault")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("DefaultItemId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("DefaultItemTypeId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("DefaultStoreId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ItemCategoryId")
                        .HasColumnType("char(36)");

                    b.Property<float>("Quantity")
                        .HasColumnType("float");

                    b.Property<int>("QuantityType")
                        .HasColumnType("int");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.PreparationStep", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Instruction")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("char(36)");

                    b.Property<int>("SortingIndex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("PreparationSteps");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("NumberOfServings")
                        .HasColumnType("int");

                    b.Property<DateTime>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.Property<Guid?>("SideDishId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("SideDishId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.TagsForRecipe", b =>
                {
                    b.Property<Guid>("RecipeId")
                        .HasColumnType("char(36)")
                        .HasColumnOrder(1);

                    b.Property<Guid>("RecipeTagId")
                        .HasColumnType("char(36)")
                        .HasColumnOrder(2);

                    b.HasKey("RecipeId", "RecipeTagId");

                    b.ToTable("TagsForRecipes");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Ingredient", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.PreparationStep", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe", "Recipe")
                        .WithMany("PreparationSteps")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe", "SideDish")
                        .WithMany()
                        .HasForeignKey("SideDishId");

                    b.Navigation("SideDish");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.TagsForRecipe", b =>
                {
                    b.HasOne("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe", "Recipe")
                        .WithMany("Tags")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe", b =>
                {
                    b.Navigation("Ingredients");

                    b.Navigation("PreparationSteps");

                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
