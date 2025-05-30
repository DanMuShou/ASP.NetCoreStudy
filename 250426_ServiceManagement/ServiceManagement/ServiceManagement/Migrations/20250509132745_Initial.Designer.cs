﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServiceManagement.Data;

#nullable disable

namespace ServiceManagement.Migrations
{
    [DbContext(typeof(ServerManagementContext))]
    [Migration("20250509132745_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ServiceManagement.Models.Server", b =>
                {
                    b.Property<int>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ServerId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ServerId");

                    b.ToTable("T_Servers", (string)null);

                    b.HasData(
                        new
                        {
                            ServerId = 2,
                            City = "Toronto",
                            IsOnline = false,
                            Name = "Server2"
                        },
                        new
                        {
                            ServerId = 1,
                            City = "Toronto",
                            IsOnline = true,
                            Name = "Server1"
                        },
                        new
                        {
                            ServerId = 3,
                            City = "Toronto",
                            IsOnline = true,
                            Name = "Server3"
                        },
                        new
                        {
                            ServerId = 4,
                            City = "Toronto",
                            IsOnline = false,
                            Name = "Server4"
                        },
                        new
                        {
                            ServerId = 5,
                            City = "Montreal",
                            IsOnline = false,
                            Name = "Server5"
                        },
                        new
                        {
                            ServerId = 6,
                            City = "Montreal",
                            IsOnline = true,
                            Name = "Server6"
                        },
                        new
                        {
                            ServerId = 7,
                            City = "Montreal",
                            IsOnline = false,
                            Name = "Server7"
                        },
                        new
                        {
                            ServerId = 8,
                            City = "Ottawa",
                            IsOnline = true,
                            Name = "Server8"
                        },
                        new
                        {
                            ServerId = 9,
                            City = "Ottawa",
                            IsOnline = false,
                            Name = "Server9"
                        },
                        new
                        {
                            ServerId = 10,
                            City = "Calgary",
                            IsOnline = false,
                            Name = "Server10"
                        },
                        new
                        {
                            ServerId = 11,
                            City = "Calgary",
                            IsOnline = false,
                            Name = "Server11"
                        },
                        new
                        {
                            ServerId = 12,
                            City = "Halifax",
                            IsOnline = false,
                            Name = "Server12"
                        },
                        new
                        {
                            ServerId = 13,
                            City = "Halifax",
                            IsOnline = false,
                            Name = "Server13"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
