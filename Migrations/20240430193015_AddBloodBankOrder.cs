using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ikagai.Migrations
{
    /// <inheritdoc />
    public partial class AddBloodBankOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodBankOrders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BloodBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsBloodBankAccept = table.Column<bool>(type: "bit", nullable: true),
                    IsCustomerAccept = table.Column<bool>(type: "bit", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quanitty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodBankOrders", x => new { x.OrderId, x.BloodBankId });
                    table.ForeignKey(
                        name: "FK_BloodBankOrders_BloodBanks_BloodBankId",
                        column: x => x.BloodBankId,
                        principalTable: "BloodBanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BloodBankOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodBankOrders_BloodBankId",
                table: "BloodBankOrders",
                column: "BloodBankId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodBankOrders");
        }
    }
}
