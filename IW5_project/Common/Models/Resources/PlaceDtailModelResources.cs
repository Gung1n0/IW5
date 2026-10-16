namespace IW5_project.Common.Models.Resources
{
    public static class PlaceDetailModelResources
    {
        public static string PlaceNameMissingErrorMessage { get { return "Name is required."; } }
        public static string PlaceNameMaximumLengthExceededErrorMessage { get { return $"Name cannot exceed {{Constants.MAX_NAME_LENGTH}} characters."; } }
        public static string PlaceDescriptionMaximumLengthExceededErrorMessage { get { return $"Description cannot exceed {{Constants.MAX_DESCRIPTION_LENGTH}} characters."; } }
        public static string PlaceItemCountNegativeErrorMessage { get { return "Item count cannot be negative."; } }

        
        public static string QueryPageGreaterThanZeroErrorMessage { get { return "Page number must be greater than zero."; } }
        public static string QueryPageSizeNotWithinRangeErrorMessage { get { return "Page size must be greater than zero, and smaller than or equal to {Constants.MAX_PAGE_SIZE}."; } }
        public static string QueryInvalidSortByErrorMessage { get { return "Sort direction must be Asc or Desc."; } }
        public static string QuerySearchTextExceedsMaxLengthErrorMessage { get { return $"Search text cannot exceed {{Constants.MAX_SEARCH_LENGTH}} characters."; } }
        public static string QueryInvalidSortFieldErrorMessage { get { return "Sort field must be Name or ItemCount."; } }
        public static string QueryOwnerIdCannotBeEmptyErrorMessage { get { return "Owner id cannot be empty when provided."; } }
    }
}
