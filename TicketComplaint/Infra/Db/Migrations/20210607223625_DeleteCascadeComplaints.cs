using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketComplaint.Infra.Db.Migrations
{
    public partial class DeleteCascadeComplaints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Tickets_TicketId",
                table: "Complaints");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Tickets_TicketId",
                table: "Complaints",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Tickets_TicketId",
                table: "Complaints");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Tickets_TicketId",
                table: "Complaints",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
