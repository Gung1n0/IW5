using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Common.Models.Resources
{
    public static class ShoppingListDetailModelResources
    {
        public static string ShoppingListNameMissingErrorMessage => "Shopping list name is required.";
        public static string ShoppingListNameMaximumLengthExceededErrorMessage => $"Shopping list name cannot exceed {Constants.MAX_NAME_LENGTH} characters.";
        public static string ShoppingListDescriptionMaximumLengthExceededErrorMessage => $"Shopping list description cannot exceed {Constants.MAX_DESCRIPTION_LENGTH} characters.";
        public static string ShoppingListQuantityNegativeErrorMessage => "Shopping list quantity cannot be negative.";
    }
}
