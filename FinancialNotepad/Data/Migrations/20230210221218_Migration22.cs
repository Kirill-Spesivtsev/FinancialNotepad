﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialNotepad.Data.Migrations
{
    public partial class Migration22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateStart",
                table: "Subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "Subscriptions");
        }
    }
}
