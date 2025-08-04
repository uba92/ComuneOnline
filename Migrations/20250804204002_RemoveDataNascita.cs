using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComuneOnline.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDataNascita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataNascita",
                table: "cittadini");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascita",
                table: "cittadini",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
