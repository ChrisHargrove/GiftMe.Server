﻿// <auto-generated />
using System;
using DataLayer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240517201031_add_gift_lists")]
    partial class add_gift_lists
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DataLayer.Database.Schema.GiftList.GiftIdeaSchemaModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<int?>("Cost")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("GiftListId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("GiftListSchemaModelId")
                        .HasColumnType("uuid");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<bool?>("IsPurchased")
                        .HasColumnType("boolean");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id");

                    b.HasIndex("GiftListId");

                    b.HasIndex("GiftListSchemaModelId");

                    b.ToTable("GiftIdeas");
                });

            modelBuilder.Entity("DataLayer.Database.Schema.GiftList.GiftListSchemaModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AccountSchemaModelId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("AccountSchemaModelId");

                    b.ToTable("GiftLists");
                });

            modelBuilder.Entity("DataLayer.Database.Schema.Identity.AccountSchemaModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("DataLayer.Database.Schema.GiftList.GiftIdeaSchemaModel", b =>
                {
                    b.HasOne("DataLayer.Database.Schema.GiftList.GiftListSchemaModel", "GiftList")
                        .WithMany()
                        .HasForeignKey("GiftListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataLayer.Database.Schema.GiftList.GiftListSchemaModel", null)
                        .WithMany("GiftIdeas")
                        .HasForeignKey("GiftListSchemaModelId");

                    b.Navigation("GiftList");
                });

            modelBuilder.Entity("DataLayer.Database.Schema.GiftList.GiftListSchemaModel", b =>
                {
                    b.HasOne("DataLayer.Database.Schema.Identity.AccountSchemaModel", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataLayer.Database.Schema.Identity.AccountSchemaModel", null)
                        .WithMany("GiftLists")
                        .HasForeignKey("AccountSchemaModelId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("DataLayer.Database.Schema.GiftList.GiftListSchemaModel", b =>
                {
                    b.Navigation("GiftIdeas");
                });

            modelBuilder.Entity("DataLayer.Database.Schema.Identity.AccountSchemaModel", b =>
                {
                    b.Navigation("GiftLists");
                });
#pragma warning restore 612, 618
        }
    }
}
