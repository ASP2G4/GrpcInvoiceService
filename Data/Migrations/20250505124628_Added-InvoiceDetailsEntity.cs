using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedInvoiceDetailsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceDetailsId",
                table: "Invoices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InvoiceDetailsEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TicketCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TicketPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AmountOfTickets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetailsEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceDetailsId",
                table: "Invoices",
                column: "InvoiceDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_InvoiceDetailsEntity_InvoiceDetailsId",
                table: "Invoices",
                column: "InvoiceDetailsId",
                principalTable: "InvoiceDetailsEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_InvoiceDetailsEntity_InvoiceDetailsId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "InvoiceDetailsEntity");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_InvoiceDetailsId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceDetailsId",
                table: "Invoices");
        }
    }
}
