using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStorage.Migrations
{
    /// <inheritdoc />
    public partial class UndoReverseRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistTrack");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtistTrack",
                columns: table => new
                {
                    ArtistsArtistId = table.Column<int>(type: "integer", nullable: false),
                    TracksTrackId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrack", x => new { x.ArtistsArtistId, x.TracksTrackId });
                    table.ForeignKey(
                        name: "FK_ArtistTrack_Artists_ArtistsArtistId",
                        column: x => x.ArtistsArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistTrack_Tracks_TracksTrackId",
                        column: x => x.TracksTrackId,
                        principalTable: "Tracks",
                        principalColumn: "TrackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrack_TracksTrackId",
                table: "ArtistTrack",
                column: "TracksTrackId");
        }
    }
}
