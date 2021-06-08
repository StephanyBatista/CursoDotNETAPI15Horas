using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketComplaint.Infra.Db.Migrations
{
    public partial class AddColumnResolved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Resolved",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resolved",
                table: "Tickets");
        }
    }
}
