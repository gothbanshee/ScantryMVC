using System;
using System.Collections.Generic;
using System.Text;

namespace Scantry.GCommon.Messages
{
    public static class HouseholdProductMessages
    {
        /* Product lists */
        public const string NoProductsForHousehold = "You haven't added any items yet. Start scanning!";
        public const string NoLowStockProducts = "You have no products that need restocking.";
        public const string NoFavouriteProducts = "You haven't marked any product as favourite.";
        public const string NoExpiringProducts = "All your items are fresh and will not expire in the next 10 days.";

        /* Favourites */
        public const string FavouriteUpdateSuccess = "Successfully changed the Favourite status!";

        /* Add householdProduct */
        public const string ProductAddedToHousehold = "Product found and added to your inventory!";

        /* Edit householdProduct */
        public const string HouseholdProductNotFound = "Product not found in the household.";
        public const string ProductQuantityNegative = "Product quantity must be a positive number.";
        public const string ProductThresholdNegative = "Minimum threshold must be a positive number.";
        public const string UpdateProductSuccess = "Product updated successfully!";

        /* Delete householdProduct */
        public const string DeleteProductConfirmation = "Are you sure that you want to delete the product from the household";
        public const string DeleteProductSuccess = "You have successfully deleted the product!";

    }
}