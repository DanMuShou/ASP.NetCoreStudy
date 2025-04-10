﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(PersonsDbContext))]
    partial class PersonsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("T_Countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryId = new Guid("a1fbb6a1-2e88-3f95-d893-1bda125d06be"),
                            CountryName = "United States"
                        },
                        new
                        {
                            CountryId = new Guid("084f5f6e-c45a-a1e5-a728-8b63aa7e0518"),
                            CountryName = "Germany"
                        },
                        new
                        {
                            CountryId = new Guid("72f28341-c912-18b2-2d5d-af0fa1391dda"),
                            CountryName = "Japan"
                        },
                        new
                        {
                            CountryId = new Guid("8206fa09-cfa7-ad82-6d7a-2d9d4211a7e7"),
                            CountryName = "Brazil"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PersonName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("ReceiveNewsLetters")
                        .HasColumnType("bit");

                    b.Property<string>("TIN")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(8)")
                        .HasDefaultValue("ABC00000")
                        .HasColumnName("TaxIdentificationNumber");

                    b.HasKey("PersonId");

                    b.ToTable("T_Persons", null, t =>
                        {
                            t.HasCheckConstraint("CHK_TIN", "LEN([TaxIdentificationNumber]) = 8");
                        });

                    b.HasData(
                        new
                        {
                            PersonId = new Guid("7b2a5666-509d-d753-6d7f-30bdf2c94a14"),
                            Address = "New York",
                            CountryId = new Guid("084f5f6e-c45a-a1e5-a728-8b63aa7e0518"),
                            DateOfBirth = new DateTime(1985, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "emily.johnson@gmail.com",
                            Gender = "Female",
                            PersonName = "Emily Johnson",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("39d2e021-9d48-77a0-a5b0-c7c28163a022"),
                            Address = "Toronto",
                            CountryId = new Guid("8206fa09-cfa7-ad82-6d7a-2d9d4211a7e7"),
                            DateOfBirth = new DateTime(1992, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "michael.b@yahoo.com",
                            Gender = "Male",
                            PersonName = "Michael Brown",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("44bcae59-6df2-137e-8606-4ade5351dff4"),
                            Address = "Paris",
                            CountryId = new Guid("72f28341-c912-18b2-2d5d-af0fa1391dda"),
                            DateOfBirth = new DateTime(1978, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "sophie.m@outlook.com",
                            Gender = "Female",
                            PersonName = "Sophie Martin",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("5e0f1c84-5719-5ef4-6bc9-dc7147bc1fed"),
                            Address = "Berlin",
                            CountryId = new Guid("a1fbb6a1-2e88-3f95-d893-1bda125d06be"),
                            DateOfBirth = new DateTime(2000, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "liam.mueller@proton.me",
                            Gender = "Male",
                            PersonName = "Liam Müller",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("452f18ce-28e7-1719-f967-83507a0c26d1"),
                            Address = "Osaka",
                            CountryId = new Guid("8206fa09-cfa7-ad82-6d7a-2d9d4211a7e7"),
                            DateOfBirth = new DateTime(1995, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "aiko.t@mail.jp",
                            Gender = "Female",
                            PersonName = "Aiko Tanaka",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("1bf25fd6-fa33-8d35-d09d-9d29cb71ab4d"),
                            Address = "Chicago",
                            CountryId = new Guid("a1fbb6a1-2e88-3f95-d893-1bda125d06be"),
                            DateOfBirth = new DateTime(1988, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "daniel.w@icloud.com",
                            Gender = "Male",
                            PersonName = "Daniel Wilson",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("3c8eb801-06f6-565f-eb2b-1732c2dd990d"),
                            Address = "Vancouver",
                            CountryId = new Guid("72f28341-c912-18b2-2d5d-af0fa1391dda"),
                            DateOfBirth = new DateTime(2002, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "emma_davis@workmail.com",
                            Gender = "Female",
                            PersonName = "Emma Davis",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("306dd088-b781-c200-9c34-89f2bbb439fb"),
                            Address = "Lyon",
                            CountryId = new Guid("084f5f6e-c45a-a1e5-a728-8b63aa7e0518"),
                            DateOfBirth = new DateTime(1999, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "lucas.dupont@mail.fr",
                            Gender = "Male",
                            PersonName = "Lucas Dupont",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("c4ec3a97-4fa4-9b2a-1ce1-56f38ea98e6d"),
                            Address = "Munich",
                            CountryId = new Guid("72f28341-c912-18b2-2d5d-af0fa1391dda"),
                            DateOfBirth = new DateTime(1982, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "clara.f@web.de",
                            Gender = "Female",
                            PersonName = "Clara Fischer",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("8803183f-5b24-c665-90df-e090d8c30e39"),
                            Address = "Kyoto",
                            CountryId = new Guid("084f5f6e-c45a-a1e5-a728-8b63aa7e0518"),
                            DateOfBirth = new DateTime(1975, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "hiroshi.s@company.jp",
                            Gender = "Male",
                            PersonName = "Hiroshi Sato",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonId = new Guid("7a2fad64-37df-1b92-62a0-0bbfd0be90d1"),
                            Address = "San Francisco",
                            CountryId = new Guid("a1fbb6a1-2e88-3f95-d893-1bda125d06be"),
                            DateOfBirth = new DateTime(1993, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "oliviaw@fastmail.com",
                            Gender = "Female",
                            PersonName = "Olivia White",
                            ReceiveNewsLetters = true
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
