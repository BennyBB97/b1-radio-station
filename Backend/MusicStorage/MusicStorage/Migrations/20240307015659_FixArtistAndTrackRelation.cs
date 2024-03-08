using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStorage.Migrations
{
    /// <inheritdoc />
    public partial class FixArtistAndTrackRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_artists_tracks_TrackId",
                table: "artists");

            migrationBuilder.DropIndex(
                name: "IX_artists_TrackId",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "TrackId",
                table: "artists");

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
                        name: "FK_ArtistTrack_artists_ArtistsArtistId",
                        column: x => x.ArtistsArtistId,
                        principalTable: "artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistTrack_tracks_TracksTrackId",
                        column: x => x.TracksTrackId,
                        principalTable: "tracks",
                        principalColumn: "TrackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrack_TracksTrackId",
                table: "ArtistTrack",
                column: "TracksTrackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistTrack");

            migrationBuilder.AddColumn<int>(
                name: "TrackId",
                table: "artists",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_artists_TrackId",
                table: "artists",
                column: "TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_artists_tracks_TrackId",
                table: "artists",
                column: "TrackId",
                principalTable: "tracks",
                principalColumn: "TrackId");
        }
    }
}
