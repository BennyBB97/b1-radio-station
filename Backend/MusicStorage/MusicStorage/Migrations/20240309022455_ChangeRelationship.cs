using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStorage.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tracks_genres_GenreId",
                table: "tracks");

            migrationBuilder.DropTable(
                name: "ArtistTrack");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tracks",
                table: "tracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_genres",
                table: "genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_artists",
                table: "artists");

            migrationBuilder.RenameTable(
                name: "tracks",
                newName: "Tracks");

            migrationBuilder.RenameTable(
                name: "genres",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "artists",
                newName: "Artists");

            migrationBuilder.RenameIndex(
                name: "IX_tracks_GenreId",
                table: "Tracks",
                newName: "IX_Tracks_GenreId");

            migrationBuilder.AlterColumn<string>(
                name: "Titel",
                table: "Tracks",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "Tracks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genres",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Artists",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks",
                column: "TrackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "GenreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artists",
                table: "Artists",
                column: "ArtistId");

            migrationBuilder.CreateTable(
                name: "TrackArtists",
                columns: table => new
                {
                    TrackId = table.Column<int>(type: "integer", nullable: false),
                    ArtistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackArtists", x => new { x.TrackId, x.ArtistId });
                    table.ForeignKey(
                        name: "FK_TrackArtists_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackArtists_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "TrackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtists_ArtistId",
                table: "TrackArtists",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Genres_GenreId",
                table: "Tracks",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Genres_GenreId",
                table: "Tracks");

            migrationBuilder.DropTable(
                name: "TrackArtists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artists",
                table: "Artists");

            migrationBuilder.RenameTable(
                name: "Tracks",
                newName: "tracks");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "genres");

            migrationBuilder.RenameTable(
                name: "Artists",
                newName: "artists");

            migrationBuilder.RenameIndex(
                name: "IX_Tracks_GenreId",
                table: "tracks",
                newName: "IX_tracks_GenreId");

            migrationBuilder.AlterColumn<string>(
                name: "Titel",
                table: "tracks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "tracks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "genres",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "artists",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tracks",
                table: "tracks",
                column: "TrackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_genres",
                table: "genres",
                column: "GenreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_artists",
                table: "artists",
                column: "ArtistId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_tracks_genres_GenreId",
                table: "tracks",
                column: "GenreId",
                principalTable: "genres",
                principalColumn: "GenreId");
        }
    }
}
