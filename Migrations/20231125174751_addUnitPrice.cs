using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WepPha2.Migrations
{
    public partial class addUnitPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<double>(
                name: "UnitPurchasePrice",
                table: "Purchases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "UnitPurchasePrice",
                table: "Purchases");

        }
    }
}
