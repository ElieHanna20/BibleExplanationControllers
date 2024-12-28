﻿using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BibleExplanationControllers.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bible");

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "bible",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                schema: "bible",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChapterNumber = table.Column<byte>(type: "smallint", nullable: false),
                    BookId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "bible",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subtitles",
                schema: "bible",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubtitleName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ChapterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subtitles_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalSchema: "bible",
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Explanations",
                schema: "bible",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    SubtitleId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Explanations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Explanations_Subtitles_SubtitleId",
                        column: x => x.SubtitleId,
                        principalSchema: "bible",
                        principalTable: "Subtitles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Verses",
                schema: "bible",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VerseNumber = table.Column<byte>(type: "smallint", nullable: false),
                    SubtitleId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    ChapterId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Verses_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalSchema: "bible",
                        principalTable: "Chapters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Verses_Subtitles_SubtitleId",
                        column: x => x.SubtitleId,
                        principalSchema: "bible",
                        principalTable: "Subtitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Name",
                schema: "bible",
                table: "Books",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_BookId_ChapterNumber",
                schema: "bible",
                table: "Chapters",
                columns: new[] { "BookId", "ChapterNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Explanations_SubtitleId",
                schema: "bible",
                table: "Explanations",
                column: "SubtitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtitles_ChapterId_SubtitleName",
                schema: "bible",
                table: "Subtitles",
                columns: new[] { "ChapterId", "SubtitleName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Verses_ChapterId",
                schema: "bible",
                table: "Verses",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Verses_SubtitleId",
                schema: "bible",
                table: "Verses",
                column: "SubtitleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Explanations",
                schema: "bible");

            migrationBuilder.DropTable(
                name: "Verses",
                schema: "bible");

            migrationBuilder.DropTable(
                name: "Subtitles",
                schema: "bible");

            migrationBuilder.DropTable(
                name: "Chapters",
                schema: "bible");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "bible");
        }
    }
}