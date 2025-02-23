using System.Text.RegularExpressions;

namespace BibleExplanationControllers.Helpers
{
    public static class PasswordValidator
    {
        public static bool IsValid(string password, out string errorMessage)
        {
            if (password.Length < 8)
            {
                errorMessage = "Password must be at least 8 characters long.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errorMessage = "Password must contain at least one uppercase letter.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                errorMessage = "Password must contain at least one lowercase letter.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                errorMessage = "Password must contain at least one number.";
                return false;
            }

            errorMessage = "";
            return true;
        }
    }
}
