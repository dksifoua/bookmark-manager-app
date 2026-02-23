using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace bookmark_manager_app.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchVectorToBookmark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                schema: "bookmark-manager",
                table: "bookmarks",
                type: "tsvector",
                nullable: false)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Title", "Description" });

            migrationBuilder.CreateIndex(
                name: "IX_bookmarks_SearchVector",
                schema: "bookmark-manager",
                table: "bookmarks",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "gin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_bookmarks_SearchVector",
                schema: "bookmark-manager",
                table: "bookmarks");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                schema: "bookmark-manager",
                table: "bookmarks");
        }
    }
}
