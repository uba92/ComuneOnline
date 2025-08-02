using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComuneOnline.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCittadiniTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cittadini",
                table: "Cittadini");

            migrationBuilder.RenameTable(
                name: "Cittadini",
                newName: "cittadini");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cittadini",
                table: "cittadini",
                column: "CittadinoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_cittadini",
                table: "cittadini");

            migrationBuilder.RenameTable(
                name: "cittadini",
                newName: "Cittadini");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cittadini",
                table: "Cittadini",
                column: "CittadinoId");
        }
    }
}
