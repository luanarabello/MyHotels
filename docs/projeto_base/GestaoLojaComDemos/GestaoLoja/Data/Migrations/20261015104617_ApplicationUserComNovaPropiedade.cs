using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoLoja.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserComNovaPropiedade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DataNascimento",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "AspNetUsers");
        }
    }
}
