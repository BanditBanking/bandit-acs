﻿// <auto-generated />
using System;
using Bandit.ACS.NpgsqlRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bandit.ACS.NpgsqlRepository.Migrations
{
    [DbContext(typeof(NpgsqlDbContext))]
    [Migration("20230403124019_m5")]
    partial class m5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Bandit.ACS.NpgsqlRepository.Models.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<string>("ActivitySector")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<int>("ChallengeType")
                        .HasColumnType("integer");

                    b.Property<string>("Communication")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MerchantBank")
                        .HasColumnType("text");

                    b.Property<string>("MerchantCardNumber")
                        .HasColumnType("text");

                    b.Property<string>("MerchantId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MerchantName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PaymentToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RequestIp")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RequestTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}