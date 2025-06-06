﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactsManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "T_Persons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "bit", nullable: false),
                    TaxIdentificationNumber = table.Column<string>(type: "varchar(8)", nullable: true, defaultValue: "ABC00000")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Persons", x => x.PersonId);
                    table.CheckConstraint("CHK_TIN", "LEN([TaxIdentificationNumber]) = 8");
                    table.ForeignKey(
                        name: "FK_T_Persons_T_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "T_Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.InsertData(
                table: "T_Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("084f5f6e-c45a-a1e5-a728-8b63aa7e0518"), "Germany" },
                    { new Guid("72f28341-c912-18b2-2d5d-af0fa1391dda"), "Japan" },
                    { new Guid("8206fa09-cfa7-ad82-6d7a-2d9d4211a7e7"), "Brazil" },
                    { new Guid("a1fbb6a1-2e88-3f95-d893-1bda125d06be"), "United States" }
                });

            migrationBuilder.InsertData(
                table: "T_Persons",
                columns: new[] { "PersonId", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("1bf25fd6-fa33-8d35-d09d-9d29cb71ab4d"), "Chicago", new Guid("a1fbb6a1-2e88-3f95-d893-1bda125d06be"), new DateTime(1988, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "daniel.w@icloud.com", "Male", "Daniel Wilson", true },
                    { new Guid("306dd088-b781-c200-9c34-89f2bbb439fb"), "Lyon", new Guid("084f5f6e-c45a-a1e5-a728-8b63aa7e0518"), new DateTime(1999, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "lucas.dupont@mail.fr", "Male", "Lucas Dupont", true },
                    { new Guid("39d2e021-9d48-77a0-a5b0-c7c28163a022"), "Toronto", new Guid("8206fa09-cfa7-ad82-6d7a-2d9d4211a7e7"), new DateTime(1992, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "michael.b@yahoo.com", "Male", "Michael Brown", false },
                    { new Guid("3c8eb801-06f6-565f-eb2b-1732c2dd990d"), "Vancouver", new Guid("72f28341-c912-18b2-2d5d-af0fa1391dda"), new DateTime(2002, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "emma_davis@workmail.com", "Female", "Emma Davis", false },
                    { new Guid("44bcae59-6df2-137e-8606-4ade5351dff4"), "Paris", new Guid("72f28341-c912-18b2-2d5d-af0fa1391dda"), new DateTime(1978, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "sophie.m@outlook.com", "Female", "Sophie Martin", true },
                    { new Guid("452f18ce-28e7-1719-f967-83507a0c26d1"), "Osaka", new Guid("8206fa09-cfa7-ad82-6d7a-2d9d4211a7e7"), new DateTime(1995, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "aiko.t@mail.jp", "Female", "Aiko Tanaka", false },
                    { new Guid("5e0f1c84-5719-5ef4-6bc9-dc7147bc1fed"), "Berlin", new Guid("a1fbb6a1-2e88-3f95-d893-1bda125d06be"), new DateTime(2000, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "liam.mueller@proton.me", "Male", "Liam Müller", true },
                    { new Guid("7a2fad64-37df-1b92-62a0-0bbfd0be90d1"), "San Francisco", new Guid("a1fbb6a1-2e88-3f95-d893-1bda125d06be"), new DateTime(1993, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "oliviaw@fastmail.com", "Female", "Olivia White", true },
                    { new Guid("7b2a5666-509d-d753-6d7f-30bdf2c94a14"), "New York", new Guid("084f5f6e-c45a-a1e5-a728-8b63aa7e0518"), new DateTime(1985, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "emily.johnson@gmail.com", "Female", "Emily Johnson", true },
                    { new Guid("8803183f-5b24-c665-90df-e090d8c30e39"), "Kyoto", new Guid("084f5f6e-c45a-a1e5-a728-8b63aa7e0518"), new DateTime(1975, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "hiroshi.s@company.jp", "Male", "Hiroshi Sato", true },
                    { new Guid("c4ec3a97-4fa4-9b2a-1ce1-56f38ea98e6d"), "Munich", new Guid("72f28341-c912-18b2-2d5d-af0fa1391dda"), new DateTime(1982, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "clara.f@web.de", "Female", "Clara Fischer", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_Persons_CountryId",
                table: "T_Persons",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Persons");

            migrationBuilder.DropTable(
                name: "T_Countries");
        }
    }
}
