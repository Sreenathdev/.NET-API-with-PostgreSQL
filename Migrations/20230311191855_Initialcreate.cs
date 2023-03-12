using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Contact_Manager.Migrations
{
    /// <inheritdoc />
    public partial class Initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
name: "Contacts",
columns: table => new
{
    Id = table.Column<int>(nullable: false)
.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
    Salutation = table.Column<string>(maxLength: 100, nullable: false),
    FirstName = table.Column<string>(maxLength: 100, nullable: false),
    LastName = table.Column<string>(maxLength: 100, nullable: false),
    DisplayName = table.Column<string>(nullable: false),
    BirthDate = table.Column<DateTimeOffset>(nullable: true),
    CreationTimestamp = table.Column<DateTimeOffset>(nullable: false),
    LastChangeTimestamp = table.Column<DateTimeOffset>(nullable: false),
    NotifyHasBirthdaySoon = table.Column<bool>(nullable: false),
    Email = table.Column<string>(nullable: false),
    PhoneNumber = table.Column<string>(nullable: false)
},
constraints: table =>
{
    table.PrimaryKey("PK_Contacts", x => x.Id);
});
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "Contacts");
        }
    }
}
