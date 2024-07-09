using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ikagai.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryCompanyServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryCompanyServices",
                columns: table => new
                {
                    DeliveryCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsCompanyAccept = table.Column<bool>(type: "bit", nullable: true),
                    IsDeliveryCompanyAccept = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryCompanyServices", x => new { x.DeliveryCompanyId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_DeliveryCompanyServices_DeliveryCompanies_DeliveryCompanyId",
                        column: x => x.DeliveryCompanyId,
                        principalTable: "DeliveryCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryCompanyServices_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryCompanyServices_OrderId",
                table: "DeliveryCompanyServices",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryCompanyServices");
        }
    }
}
