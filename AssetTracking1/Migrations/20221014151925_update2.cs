using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetTracking1.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackInfo_CreatedBy",
                table: "Assets",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"CREATE TRIGGER update_trigger ON Assets 
                                AFTER UPDATE AS 
                                BEGIN 
                                SET NOCOUNT ON; 
                                UPDATE Assets set TrackInfo_ChangeDate = GETDATE() 
                                FROM Assets b INNER JOIN inserted i on b.AssetId=i.AssetId
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackInfo_CreatedBy",
                table: "Assets");
        }
    }
}
