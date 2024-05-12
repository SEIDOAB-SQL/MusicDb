﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class initial_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ArtistId);
                });

            migrationBuilder.CreateTable(
                name: "MusicGroups",
                columns: table => new
                {
                    MusicGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    EstablishedYear = table.Column<int>(type: "int", nullable: false),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicGroups", x => x.MusicGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    MusicGroupsMusicGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.AlbumId);
                    table.ForeignKey(
                        name: "FK_Albums_MusicGroups_MusicGroupsMusicGroupId",
                        column: x => x.MusicGroupsMusicGroupId,
                        principalTable: "MusicGroups",
                        principalColumn: "MusicGroupId");
                });

            migrationBuilder.CreateTable(
                name: "csArtistcsMusicGroup",
                columns: table => new
                {
                    MembersArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MusicGroupsMusicGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csArtistcsMusicGroup", x => new { x.MembersArtistId, x.MusicGroupsMusicGroupId });
                    table.ForeignKey(
                        name: "FK_csArtistcsMusicGroup_Artists_MembersArtistId",
                        column: x => x.MembersArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_csArtistcsMusicGroup_MusicGroups_MusicGroupsMusicGroupId",
                        column: x => x.MusicGroupsMusicGroupId,
                        principalTable: "MusicGroups",
                        principalColumn: "MusicGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_MusicGroupsMusicGroupId",
                table: "Albums",
                column: "MusicGroupsMusicGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_csArtistcsMusicGroup_MusicGroupsMusicGroupId",
                table: "csArtistcsMusicGroup",
                column: "MusicGroupsMusicGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "csArtistcsMusicGroup");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "MusicGroups");
        }
    }
}
