using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ikagai.Migrations
{
    /// <inheritdoc />
    public partial class AddTestResultAndDonationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodBankDeliveries");

            migrationBuilder.CreateTable(
                name: "DonationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastDonationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BloodBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    GovernorateId = table.Column<byte>(type: "tinyint", nullable: false),
                    CityId = table.Column<byte>(type: "tinyint", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonationRequests_BloodBanks_BloodBankId",
                        column: x => x.BloodBankId,
                        principalTable: "BloodBanks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DonationRequests_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonationRequests_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonationRequests_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonationRequests_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResultImg = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DonateAgain = table.Column<bool>(type: "bit", nullable: false),
                    DonationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BloodBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResults_BloodBanks_BloodBankId",
                        column: x => x.BloodBankId,
                        principalTable: "BloodBanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResults_DonationRequests_DonationRequestId",
                        column: x => x.DonationRequestId,
                        principalTable: "DonationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_BloodBankId",
                table: "DonationRequests",
                column: "BloodBankId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_CityId",
                table: "DonationRequests",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_GovernorateId",
                table: "DonationRequests",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_PersonId",
                table: "DonationRequests",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_StatusId",
                table: "DonationRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_BloodBankId",
                table: "TestResults",
                column: "BloodBankId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_DonationRequestId",
                table: "TestResults",
                column: "DonationRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "DonationRequests");

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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodBankDeliveries_DeliveryCompanyId",
                table: "BloodBankDeliveries",
                column: "DeliveryCompanyId");
        }
    }
}
