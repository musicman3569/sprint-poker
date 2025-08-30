using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SprintPokerApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardSets",
                columns: table => new
                {
                    CardSetId = table.Column<int>(type: "integer", nullable: false, comment: "The unique identifier for the card set.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false, comment: "The name of the card set. Limited to 36 characters."),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "The date and time when the entity was created. Defaults to the current date and time."),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "The date and time when the entity was last modified. Defaults to the current date and time."),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier of the user who created the entity."),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier of the user who last modified the entity.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardSets", x => x.CardSetId);
                },
                comment: "Represents a collection of planning poker cards that can be used for story point estimation. This can be used to create different sprint point schemes such as fibonacci or t-shirt sizes. Inherits audit properties from AuditableEntity.");

            migrationBuilder.CreateTable(
                name: "VoteFlag",
                columns: table => new
                {
                    VoteFlagId = table.Column<int>(type: "integer", nullable: false, comment: "The identifier representing the type of vote flag."),
                    Name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false, comment: "The name of the vote flag. Limited to 24 characters."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "The description of the vote flag. Limited to 255 characters.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteFlag", x => x.VoteFlagId);
                },
                comment: "Represents a vote flag entity that defines the voting behavior and its description.");

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "integer", nullable: false, comment: "The unique identifier for the card.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<int>(type: "integer", nullable: false, comment: "The numerical value of the card used for story point estimation."),
                    DisplayName = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true, comment: "The display name of the card. Limited to 24 characters."),
                    CardSetId = table.Column<int>(type: "integer", nullable: false, comment: "The identifier of the card set this card belongs to."),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "The date and time when the entity was created. Defaults to the current date and time."),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "The date and time when the entity was last modified. Defaults to the current date and time."),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier of the user who created the entity."),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier of the user who last modified the entity.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_Cards_CardSets_CardSetId",
                        column: x => x.CardSetId,
                        principalTable: "CardSets",
                        principalColumn: "CardSetId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a planning poker card entity with its value and display properties. A card belongs to a card set, and has the value that will be used for story point estimation. Inherits audit properties from AuditableEntity.");

            migrationBuilder.CreateTable(
                name: "PokerRooms",
                columns: table => new
                {
                    PokerRoomId = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier for the poker room."),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "The name of the poker room. Limited to 256 characters."),
                    CardSetId = table.Column<int>(type: "integer", nullable: false, comment: "The identifier of the card set used in this poker room."),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "The date and time when the entity was created. Defaults to the current date and time."),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "The date and time when the entity was last modified. Defaults to the current date and time."),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier of the user who created the entity."),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier of the user who last modified the entity.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokerRooms", x => x.PokerRoomId);
                    table.ForeignKey(
                        name: "FK_PokerRooms_CardSets_CardSetId",
                        column: x => x.CardSetId,
                        principalTable: "CardSets",
                        principalColumn: "CardSetId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a planning poker room where players can participate in story point estimation sessions. Inherits audit properties from AuditableEntity.");

            migrationBuilder.CreateTable(
                name: "PokerPlayers",
                columns: table => new
                {
                    PokerPlayerId = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier for the poker player."),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "The email address of the player. Limited to 256 characters."),
                    DisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "The display name of the player shown during poker sessions. Limited to 256 characters."),
                    PokerRoomCurrentId = table.Column<Guid>(type: "uuid", nullable: true, comment: "The identifier of the poker room where the player is currently present."),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "The date and time when the entity was created. Defaults to the current date and time."),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "The date and time when the entity was last modified. Defaults to the current date and time."),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier of the user who created the entity."),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier of the user who last modified the entity.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokerPlayers", x => x.PokerPlayerId);
                    table.ForeignKey(
                        name: "FK_PokerPlayers_PokerRooms_PokerRoomCurrentId",
                        column: x => x.PokerRoomCurrentId,
                        principalTable: "PokerRooms",
                        principalColumn: "PokerRoomId",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Represents a player in the planning poker session. Inherits audit properties from AuditableEntity.");

            migrationBuilder.CreateTable(
                name: "PokerPlayerRooms",
                columns: table => new
                {
                    PokerPlayersPokerPlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PokerRoomsPokerRoomId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokerPlayerRooms", x => new { x.PokerPlayersPokerPlayerId, x.PokerRoomsPokerRoomId });
                    table.ForeignKey(
                        name: "FK_PokerPlayerRooms_PokerPlayers_PokerPlayersPokerPlayerId",
                        column: x => x.PokerPlayersPokerPlayerId,
                        principalTable: "PokerPlayers",
                        principalColumn: "PokerPlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokerPlayerRooms_PokerRooms_PokerRoomsPokerRoomId",
                        column: x => x.PokerRoomsPokerRoomId,
                        principalTable: "PokerRooms",
                        principalColumn: "PokerRoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    VoteId = table.Column<Guid>(type: "uuid", nullable: false, comment: "The unique identifier for the vote."),
                    CardId = table.Column<int>(type: "integer", nullable: false, comment: "The identifier of the selected card for this vote."),
                    PokerPlayerId = table.Column<Guid>(type: "uuid", nullable: false, comment: "The identifier of the player who cast this vote."),
                    PokerRoomId = table.Column<Guid>(type: "uuid", nullable: false, comment: "The identifier of the poker room where this vote was cast."),
                    VoteFlagId = table.Column<int>(type: "integer", nullable: false, comment: "The identifier of the vote flag status.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_Votes_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_PokerPlayers_PokerPlayerId",
                        column: x => x.PokerPlayerId,
                        principalTable: "PokerPlayers",
                        principalColumn: "PokerPlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_PokerRooms_PokerRoomId",
                        column: x => x.PokerRoomId,
                        principalTable: "PokerRooms",
                        principalColumn: "PokerRoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_VoteFlag_VoteFlagId",
                        column: x => x.VoteFlagId,
                        principalTable: "VoteFlag",
                        principalColumn: "VoteFlagId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a vote cast by a player in a planning poker room. Each vote links a player's card selection to a specific poker room.");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardSetId",
                table: "Cards",
                column: "CardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_PokerPlayerRooms_PokerRoomsPokerRoomId",
                table: "PokerPlayerRooms",
                column: "PokerRoomsPokerRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PokerPlayers_Email",
                table: "PokerPlayers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PokerPlayers_PokerRoomCurrentId",
                table: "PokerPlayers",
                column: "PokerRoomCurrentId");

            migrationBuilder.CreateIndex(
                name: "IX_PokerRooms_CardSetId",
                table: "PokerRooms",
                column: "CardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_CardId",
                table: "Votes",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PokerPlayerId_PokerRoomId",
                table: "Votes",
                columns: new[] { "PokerPlayerId", "PokerRoomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PokerRoomId",
                table: "Votes",
                column: "PokerRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_VoteFlagId",
                table: "Votes",
                column: "VoteFlagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PokerPlayerRooms");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "PokerPlayers");

            migrationBuilder.DropTable(
                name: "VoteFlag");

            migrationBuilder.DropTable(
                name: "PokerRooms");

            migrationBuilder.DropTable(
                name: "CardSets");
        }
    }
}
