using System;
using System.Collections.Generic;
using System.Text;

namespace Scantry.GCommon.Messages
{
    public static class HouseholdProductMessages
    {
        public const string NoProductsForHousehold = "You haven't added any items yet. Start scanning!";
        public const string NoLowStockProducts = "You have no products that need re-stocking.";
        public const string NoFavouriteProducts = "You haven't marked any product as favourite.";
        public const string NoExpiringProducts = "All your items are fresh and will not expire in the next 10 days.";
        public const string FavouriteUpdateFail = "Could not update favourite status.";
        public const string QuantityNegative = "Quantity must be a positive number.";
        public const string ThresholdNegative = "Threshold must be a positive number.";
        public const string ChangedFavouriteStatusSuccess = "Successfully changed the Favourite status!";
        public const string DeleteConfirmation = "Are you sure that you want to delete the product from the household";
        public const string DeleteSuccess = "You have successfully deleted the product!";

    }
}