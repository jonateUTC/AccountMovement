﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountMovement.Context.ApiData
{
    public partial class StartUP2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MovementValueIni",
                table: "Movement",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovementValueIni",
                table: "Movement");
        }
    }
}
