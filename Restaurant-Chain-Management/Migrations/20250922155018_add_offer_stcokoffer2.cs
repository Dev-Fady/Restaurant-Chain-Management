using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Chain_Management.Migrations
{
    /// <inheritdoc />
    public partial class add_offer_stcokoffer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageOffer_Offer_OfferId",
                table: "ImageOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferStock_Branches_BranchId",
                table: "OfferStock");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferStock_Offer_OfferId",
                table: "OfferStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferStock",
                table: "OfferStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageOffer",
                table: "ImageOffer");

            migrationBuilder.RenameTable(
                name: "OfferStock",
                newName: "OfferStocks");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "Offers");

            migrationBuilder.RenameTable(
                name: "ImageOffer",
                newName: "ImageOffers");

            migrationBuilder.RenameIndex(
                name: "IX_OfferStock_OfferId_BranchId",
                table: "OfferStocks",
                newName: "IX_OfferStocks_OfferId_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferStock_BranchId",
                table: "OfferStocks",
                newName: "IX_OfferStocks_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_ImageOffer_OfferId",
                table: "ImageOffers",
                newName: "IX_ImageOffers_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferStocks",
                table: "OfferStocks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageOffers",
                table: "ImageOffers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageOffers_Offers_OfferId",
                table: "ImageOffers",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferStocks_Branches_BranchId",
                table: "OfferStocks",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferStocks_Offers_OfferId",
                table: "OfferStocks",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageOffers_Offers_OfferId",
                table: "ImageOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferStocks_Branches_BranchId",
                table: "OfferStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferStocks_Offers_OfferId",
                table: "OfferStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferStocks",
                table: "OfferStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageOffers",
                table: "ImageOffers");

            migrationBuilder.RenameTable(
                name: "OfferStocks",
                newName: "OfferStock");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Offer");

            migrationBuilder.RenameTable(
                name: "ImageOffers",
                newName: "ImageOffer");

            migrationBuilder.RenameIndex(
                name: "IX_OfferStocks_OfferId_BranchId",
                table: "OfferStock",
                newName: "IX_OfferStock_OfferId_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferStocks_BranchId",
                table: "OfferStock",
                newName: "IX_OfferStock_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_ImageOffers_OfferId",
                table: "ImageOffer",
                newName: "IX_ImageOffer_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferStock",
                table: "OfferStock",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageOffer",
                table: "ImageOffer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageOffer_Offer_OfferId",
                table: "ImageOffer",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferStock_Branches_BranchId",
                table: "OfferStock",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferStock_Offer_OfferId",
                table: "OfferStock",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
