using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TextboxMailApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewPropertyUid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Uid",
                table: "EmailMessages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uid",
                table: "EmailMessages");
        }
    }
}
