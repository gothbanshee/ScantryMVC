namespace Scantry.GCommon
{
    public static class HouseholdMessages
    {
        /* General */
        public const string UnauthorizedUser = "You are not a member of this household!";
        public const string NoHouseholdJoined = "You are not a member of any household!";

        /* Create household */
        public const string CreateSuccess = "You have successfully created a new household!";

        /* Rename household */
        public const string EmptyHouseholdName = "Household name cannot be empty.";
        public const string RenameSuccess = "You have successfully renamed the household!";

        /* Join household */
        public const string UserAlreadyMember = "You are already a member of this household.";
        public const string InvalidAccessToken = "Invalid access token. Please try again.";
        public const string InvalidJoinLink = "The invite link format is invalid. Please try again.";
        public const string JoinSuccess = "You have successfully joined the household!";

        /* Leave household */
        public const string LeaveWarningLastUser = "There are no other members in the household, leaving will also delete the household and all of its products. Continue?";
        public const string LeaveConfirmation = "Are you sure that you want to leave the household?";
        public const string LeaveSuccess = "You have successfully left the household!";

        /* Delete household */
        public const string DeleteConfirmation = "Are you sure that you want to delete the household and all of its products?";
        public const string DeleteSuccess = "You have successfully deleted the household and all of its products!";
    }
}