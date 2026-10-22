namespace IW5_project.Common.Models.Resources
{
    public static class ItemDetailModelResources
    {
        public static string ItemNameMissingErrorMessage { get { return "Item name required"; } }
        public static string ItemNameMaximumLengthExceededErrorMessage { get { return $"Item name cannot exceed {{Constants.MAX_NAME_LENGTH}} characters."; } }
        public static string ItemDescriptionMaximumLengthExceededErrorMessage { get { return $"Item description cannot exceed {{Constants.MAX_DESCRIPTION_LENGTH}} characters."; } }
        public static string ItemUrlMaximumLenghtExceededErrorMessage { get { return $"Item url cannot exceed {{Constans.MAX_URL_LENGHT}} characters."; } }
        public static string ItemQuantityPositiveErrorMessage { get { return "Quantity cant be negative."; } }
        public static string ItemIdEmptyErrorMessage { get { return "Id cant be empty"; } }
    }
}
