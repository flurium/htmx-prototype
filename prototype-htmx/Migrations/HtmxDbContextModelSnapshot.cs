﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using htmx_prototype.Data;

#nullable disable

namespace prototype_htmx.Migrations
{
    [DbContext(typeof(HtmxDbContext))]
    partial class HtmxDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.31");

            modelBuilder.Entity("htmx.Models.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PreviewImage")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = "7bad0bde-7597-4e77-a8f5-2502850a7b2b",
                            Description = "This is a test product",
                            Name = "Test Product 1",
                            PreviewImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/Tomato_je.jpg/1200px-Tomato_je.jpg",
                            Price = 19.989999999999998
                        },
                        new
                        {
                            Id = "9ec8fc3d-b38d-427b-9325-41fcfbce0961",
                            Description = "This is another test product",
                            Name = "Test Product 2",
                            PreviewImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/Tomato_je.jpg/1200px-Tomato_je.jpg",
                            Price = 29.989999999999998
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
