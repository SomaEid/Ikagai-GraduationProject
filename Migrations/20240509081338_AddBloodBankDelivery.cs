using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ikagai.Migrations
{
    /// <inheritdoc />
    public partial class AddBloodBankDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodBankDeliveries",
                columns: table => new
                {
                    BloodBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryComapnyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodBankDeliveries", x => new { x.BloodBankId, x.DeliveryComapnyId });
                    table.ForeignKey(
                        name: "FK_BloodBankDeliveries_BloodBanks_BloodBankId",
                        column: x => x.BloodBankId,
                        principalTable: "BloodBanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BloodBankDeliveries_DeliveryCompanies_DeliveryCompanyId",
                        column: x => x.DeliveryCompanyId,
                        principalTable: "DeliveryCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodBankDeliveries_DeliveryCompanyId",
                table: "BloodBankDeliveries",
                column: "DeliveryCompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodBankDeliveries");
        }
    }
}
