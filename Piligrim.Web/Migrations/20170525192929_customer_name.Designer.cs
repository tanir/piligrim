using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Piligrim.Data;
using Piligrim.Core.Models;

namespace Piligrim.Web.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20170525192929_customer_name")]
    partial class customer_name
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

            modelBuilder.Entity("Piligrim.Core.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Comment");

                    b.Property<string>("CustomerName");

                    b.Property<int>("Delivery");

                    b.Property<string>("DeliveryComment");

                    b.Property<string>("Email");

                    b.Property<int>("Payment");

                    b.Property<string>("PhoneNumber");

                    b.Property<int>("Status");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Piligrim.Core.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<int>("Count");

                    b.Property<int?>("OrderId");

                    b.Property<decimal>("Price");

                    b.Property<int?>("ProductId");

                    b.Property<string>("Size");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItem");
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

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<decimal>("Price");

                    b.Property<string>("Thumbnail");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAddOrUpdate();

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

            modelBuilder.Entity("Piligrim.Core.Models.OrderItem", b =>
                {
                    b.HasOne("Piligrim.Core.Models.Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");

                    b.HasOne("Piligrim.Core.Models.Product", "Product")
                        .WithMany()
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
