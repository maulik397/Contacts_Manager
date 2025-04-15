using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
  public partial class StoredProc : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
            string sp_Create = @"
        IF OBJECT_ID('dbo.GetAllPersons', 'P') IS NULL
        EXEC('CREATE PROCEDURE [dbo].[GetAllPersons] AS BEGIN SET NOCOUNT ON; SELECT 1; END')
      ";
            migrationBuilder.Sql(sp_Create);

            // Step 2: Alter it to the actual implementation
            string sp_Alter = @"
        ALTER PROCEDURE [dbo].[GetAllPersons]
        AS BEGIN
          SELECT PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters 
          FROM [dbo].[Persons]
        END
      ";
            migrationBuilder.Sql(sp_Alter);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
    {
      string sp_GetAllPersons = @"
        DROP PROCEDURE [dbo].[GetAllPersons]
      ";
      migrationBuilder.Sql(sp_GetAllPersons);
    }
  }
}
