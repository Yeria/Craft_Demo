using Microsoft.EntityFrameworkCore.Migrations;

namespace CraftBackEnd.Migrations.Migrations
{
    public partial class FixIncorrectPropertyDecorators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "varchar",
                table: "User",
                newName: "MemberType");

            migrationBuilder.AlterColumn<string>(
                name: "MemberType",
                table: "User",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "User",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "MemberType",
                table: "User",
                newName: "varchar");

            migrationBuilder.AlterColumn<string>(
                name: "varchar",
                table: "User",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5);
        }
    }
}
