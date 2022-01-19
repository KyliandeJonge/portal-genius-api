using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalGenius.Infrastructure.Data.Migrations.PostgreSQL;

public partial class InitialPql : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Items",
            table => new
            {
                Id = table.Column<string>("text", nullable: false),
                Owner = table.Column<string>("text", nullable: true),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Title = table.Column<string>("text", nullable: true),
                Type = table.Column<string>("text", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_Items", x => x.Id); });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Items");
    }
}
