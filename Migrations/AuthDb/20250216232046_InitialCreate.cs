using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BibleExplanationControllers.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    UserType = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    CanChangeBooksData = table.Column<bool>(type: "boolean", nullable: true),
                    AdminId = table.Column<Guid>(type: "uuid", nullable: true),
                    Worker_CanChangeBooksData = table.Column<bool>(type: "boolean", nullable: true),
                    SubAdminId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Users_SubAdminId",
                        column: x => x.SubAdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChapterNumber = table.Column<byte>(type: "smallint", nullable: false),
                    BookId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapter_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subtitle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubtitleName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ChapterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subtitle_Chapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Explanation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    SubtitleId = table.Column<int>(type: "integer", nullable: true),
                    SubAdminId = table.Column<Guid>(type: "uuid", nullable: true),
                    WorkerId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Explanation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Explanation_Subtitle_SubtitleId",
                        column: x => x.SubtitleId,
                        principalTable: "Subtitle",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Explanation_Users_SubAdminId",
                        column: x => x.SubAdminId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Explanation_Users_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Verse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VerseNumber = table.Column<byte>(type: "smallint", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    SubtitleId = table.Column<int>(type: "integer", nullable: false),
                    ChapterId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Verse_Chapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Verse_Subtitle_SubtitleId",
                        column: x => x.SubtitleId,
                        principalTable: "Subtitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_BookId",
                table: "Chapter",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Explanation_SubAdminId",
                table: "Explanation",
                column: "SubAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Explanation_SubtitleId",
                table: "Explanation",
                column: "SubtitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Explanation_WorkerId",
                table: "Explanation",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtitle_ChapterId",
                table: "Subtitle",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AdminId",
                table: "Users",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubAdminId",
                table: "Users",
                column: "SubAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Verse_ChapterId",
                table: "Verse",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Verse_SubtitleId",
                table: "Verse",
                column: "SubtitleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Explanation");

            migrationBuilder.DropTable(
                name: "Verse");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Subtitle");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "Book");
        }
    }
}
