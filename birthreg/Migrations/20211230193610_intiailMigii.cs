using Microsoft.EntityFrameworkCore.Migrations;

namespace birthreg.Migrations
{
    public partial class intiailMigii : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FamilyName",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherPhoneNumber",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherPhoneNumber",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FamilyName",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "FatherPhoneNumber",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "MotherPhoneNumber",
                table: "Parents");
        }
    }
}
