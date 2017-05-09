using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Piligrim.Data;

namespace Piligrim.Web.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    [Migration("20170509143319_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Piligrim.Core.Models.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ProductId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Color");
                });

            modelBuilder.Entity("Piligrim.Core.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ProductId");

                    b.Property<string>("Uri");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("Piligrim.Core.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("Description");

                    b.Property<decimal>("Price");

                    b.Property<string>("Thumbnail");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Piligrim.Core.Models.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ProductId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Size");
                });

            modelBuilder.Entity("Piligrim.Core.Models.Color", b =>
                {
                    b.HasOne("Piligrim.Core.Models.Product")
                        .WithMany("Colors")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Piligrim.Core.Models.Photo", b =>
                {
                    b.HasOne("Piligrim.Core.Models.Product")
                        .WithMany("Photos")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Piligrim.Core.Models.Size", b =>
                {
                    b.HasOne("Piligrim.Core.Models.Product")
                        .WithMany("Sizes")
                        .HasForeignKey("ProductId");
                });
        }
    }
}
