using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalGenius.Infrastructure.Data.Migrations.SQLServer;

public partial class InitialSQServer : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Items",
            table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                Owner = table.Column<string>("nvarchar(max)", nullable: true),
                Created = table.Column<DateTime>("datetime2", nullable: false),
                Title = table.Column<string>("nvarchar(max)", nullable: true),
                Type = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_Items", x => x.Id); });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Items");
    }
}
