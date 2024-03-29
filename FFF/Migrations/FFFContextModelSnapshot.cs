using Microsoft.EntityFrameworkCore.Migrations;
using System;

public partial class CreateFFFDatabase : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Events",
            columns: table => new
            {
                Id = table.Column<long>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Date = table.Column<DateTime>(nullable: false),
                SingerName = table.Column<string>(maxLength: 50, nullable: false),
                TicketPrice = table.Column<decimal>(nullable: false),
                Description = table.Column<string>(maxLength: 500, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Events", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Employees",
            columns: table => new
            {
                Id = table.Column<long>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>(maxLength: 20, nullable: false),
                LastName = table.Column<string>(maxLength: 20, nullable: false),
                BirthDate = table.Column<DateTime>(nullable: false),
                Position = table.Column<int>(nullable: false),
                PhoneNumber = table.Column<string>(nullable: false),
                Email = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Employees", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Reservations",
            columns: table => new
            {
                Id = table.Column<long>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(maxLength: 50, nullable: false),
                PhoneNumber = table.Column<string>(nullable: false),
                Note = table.Column<string>(maxLength: 500, nullable: true),
                EventId = table.Column<long>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Reservations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Reservations_Events_EventId",
                    column: x => x.EventId,
                    principalTable: "Events",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "EmployeeEvent",
            columns: table => new
            {
                EventId = table.Column<long>(nullable: false),
                EmployeeId = table.Column<long>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmployeeEvent", x => new { x.EventId, x.EmployeeId });
                table.ForeignKey(
                    name: "FK_EmployeeEvent_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_EmployeeEvent_Events_EventId",
                    column: x => x.EventId,
                    principalTable: "Events",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeEvent_EmployeeId",
            table: "EmployeeEvent",
            column: "EmployeeId");

        migrationBuilder.CreateIndex(
            name: "IX_Reservations_EventId",
            table: "Reservations",
            column: "EventId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EmployeeEvent");

        migrationBuilder.DropTable(
            name: "Reservations");

        migrationBuilder.DropTable(
            name: "Employees");

        migrationBuilder.DropTable(
            name: "Events");
    }
}
