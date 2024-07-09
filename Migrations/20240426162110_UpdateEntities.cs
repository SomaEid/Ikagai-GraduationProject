using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ikagai.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodBanks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodBankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpenHour = table.Column<TimeOnly>(type: "time", nullable: false),
                    ClosedHour = table.Column<TimeOnly>(type: "time", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CityId = table.Column<byte>(type: "tinyint", nullable: false),
                    GovernorateId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodBanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodBanks_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BloodBanks_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BloodBanks_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BloodBanks_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpenHour = table.Column<TimeOnly>(type: "time", nullable: false),
                    ClosedHour = table.Column<TimeOnly>(type: "time", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommercialRegister = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CityId = table.Column<byte>(type: "tinyint", nullable: false),
                    GovernorateId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryCompanies_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryCompanies_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryCompanies_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BloodAndDerivativesId = table.Column<byte>(type: "tinyint", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<byte>(type: "tinyint", nullable: false),
                    GovernorateId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_People_BloodAndDerivatives_BloodAndDerivativesId",
                        column: x => x.BloodAndDerivativesId,
                        principalTable: "BloodAndDerivatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_People_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_People_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodBanks_ApplicationUserId",
                table: "BloodBanks",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodBanks_CityId",
                table: "BloodBanks",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodBanks_GovernorateId",
                table: "BloodBanks",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodBanks_StatusId",
                table: "BloodBanks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryCompanies_ApplicationUserId",
                table: "DeliveryCompanies",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryCompanies_CityId",
                table: "DeliveryCompanies",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryCompanies_GovernorateId",
                table: "DeliveryCompanies",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_People_ApplicationUserId",
                table: "People",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_People_BloodAndDerivativesId",
                table: "People",
                column: "BloodAndDerivativesId");

            migrationBuilder.CreateIndex(
                name: "IX_People_CityId",
                table: "People",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_People_GovernorateId",
                table: "People",
                column: "GovernorateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodBanks");

            migrationBuilder.DropTable(
                name: "DeliveryCompanies");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
