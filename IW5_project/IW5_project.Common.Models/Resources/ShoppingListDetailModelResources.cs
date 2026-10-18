using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Common.Models.Resources
{
    public static class ShoppingListDetailModelResources
    {
        public static string ShoppingListNameMissingErrorMessage { get { return "Shopping list name is required."; } }
        public static string ShoppingListNameMaximumLengthExceededErrorMessage { get { return $"Shopping list name cannot exceed {{Constants.MAX_NAME_LENGTH}} characters."; } }
        public static string ShoppingListDescriptionMaximumLengthExceededErrorMessage { get { return $"Shopping list description cannot exceed {{Constants.MAX_DESCRIPTION_LENGTH}} characters."; } }
        public static string ShoppingListQuantityNegativeErrorMessage { get { return "Shopping list quantity cannot be negative."; } }
        public static string ItemIdRequiredErrorMessage { get { return "Item ID is required."; } }
        public static string ShoppingListQuantityNotWithinRangeErrorMessage { get { return $"Shopping list quantity must be between 0 and {{Constants.MAX_QUANTITY}}."; } }
        public static string ShoppingListStoreMissingErrorMessage { get { return "Shopping list store is required."; } }
        public static string ShoppingListStoreMaximumLengthExceededErrorMessage { get { return $"Shopping list store cannot exceed {{Constants.MAX_NAME_LENGTH}} characters."; } }

        public static string QueryOwnerIdCannotBeEmptyErrorMessage { get { return "Owner ID cannot be empty."; } }
        public static string QueryStoreExceedsMaxLengthErrorMessage { get { return $"Store name cannot exceed {{Constants.MAX_NAME_LENGTH}} characters."; } }
        public static string QueryInvalidSortFieldErrorMessage { get { return "Sort field must be Name, Store or ItemCount."; } }

    }
}
