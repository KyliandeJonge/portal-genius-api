using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalGenius.Infrastructure.Data.Migrations.SQLite;

public partial class InitialSQLite : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Items",
            table => new
            {
                Id = table.Column<string>("TEXT", nullable: false),
                Owner = table.Column<string>("TEXT", nullable: true),
                Created = table.Column<DateTime>("TEXT", nullable: false),
                Title = table.Column<string>("TEXT", nullable: true),
                Type = table.Column<string>("TEXT", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_Items", x => x.Id); });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Items");
    }
}
