using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelStationManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FuelStationMigrationV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    TCKN = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    NameSurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.TCKN);
                });

            migrationBuilder.CreateTable(
                name: "Balance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CustomerTCKN = table.Column<string>(type: "nvarchar(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Balance_Customer_CustomerTCKN",
                        column: x => x.CustomerTCKN,
                        principalTable: "Customer",
                        principalColumn: "TCKN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Plate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    FuelType = table.Column<int>(type: "int", nullable: false),
                    CustomerTCKN = table.Column<string>(type: "nvarchar(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Plate);
                    table.ForeignKey(
                        name: "FK_Vehicle_Customer_CustomerTCKN",
                        column: x => x.CustomerTCKN,
                        principalTable: "Customer",
                        principalColumn: "TCKN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuelTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehiclePlate = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelTransaction_Vehicle_VehiclePlate",
                        column: x => x.VehiclePlate,
                        principalTable: "Vehicle",
                        principalColumn: "Plate",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Balance_CustomerTCKN",
                table: "Balance",
                column: "CustomerTCKN");

            migrationBuilder.CreateIndex(
                name: "IX_FuelTransaction_VehiclePlate",
                table: "FuelTransaction",
                column: "VehiclePlate");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_CustomerTCKN",
                table: "Vehicle",
                column: "CustomerTCKN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Balance");

            migrationBuilder.DropTable(
                name: "FuelTransaction");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
