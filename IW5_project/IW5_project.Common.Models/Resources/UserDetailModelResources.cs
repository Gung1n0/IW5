namespace IW5_project.Common.Models.Resources
{
    public static class UserDetailModelResources
    {
        public static string UserNameMissingErrorMessage => "User name is required.";
        public static string UserNameMaximumLengthExceededErrorMessage => $"Name cannot exceed {Constants.MAX_NAME_LENGTH} characters.";
        public static string UserRoleMissingErrorMessage => "Role is required.";
        public static string UserRoleInvalidErrorMessage => "Role must be either 'user' or 'admin'.";
        public static string UserPhotoUrlMaximumLengthExceededErrorMessage => $"PhotoUrl cannot exceed {Constants.MAX_IMAGE_URL_LENGTH} characters.";
    }
}