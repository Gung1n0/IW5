namespace IW5_project.Common.Models.Resources
{
    public static class PlaceDetailModelResources
    {
        public static string NameMissingErrorMessage { get { return "Name is required."; } }
        public static string MaximumLengthExceededErrorMessage { get { return $"Name cannot exceed {{Constants.MAX_NAME_LENGTH}} characters."; } }
        public static string DescriptionMaximumLengthExceededErrorMessage { get { return $"Description cannot exceed {{Constants.MAX_DESCRIPTION_LENGTH}} characters."; } }
        public static string ItemCountNegativeErrorMessage { get { return "Item count cannot be negative."; } }
    }
}
