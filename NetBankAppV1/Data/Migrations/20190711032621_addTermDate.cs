using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBankAppV1.Data.Migrations
{
    public partial class addTermDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "customers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<DateTime>(
                name: "TermEndedDate",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TermEndedDate",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "customers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
