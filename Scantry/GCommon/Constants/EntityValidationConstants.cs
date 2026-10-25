namespace Scantry.GCommon.Constants
{
    public static class EntityValidationConstants
    {
        /* Household */
        public const int HouseholdNameMinLength = 1;
        public const int HouseholdNameMaxLength = 50;

        /* Product */
        public const int BarcodeMinLength = 12;
        public const int BarcodeMaxLength = 50;
        public const int ProductTypeMinLength = 1;
        public const int ProductTypeMaxLength = 75;
        public const int BrandMinLength = 1;
        public const int BrandMaxLength = 100;
        public const int ModelMinLength = 1;
        public const int ModelMaxLength = 150;
        public const int VariantMaxLength = 100;
        public const int SizeUnitMinLength = 1;
        public const int SizeUnitMaxLength = 10; // change if I add a longer SizeUnit string

        /* Household product */
        public const int CategoryMinLength = 4;
        public const int CategoryMaxLength = 20;
        public const int ExpirationDateCheck = 10;
    }
}