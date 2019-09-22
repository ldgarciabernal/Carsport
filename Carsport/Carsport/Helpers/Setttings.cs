namespace Carsport.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string username = "Username";
        private const string password = "Password";
        private const string tokenType = "Tokentype";
        private const string accessToken = "AccessToken";
        private const string isRemembered = "IsRemembered";
        private const string userASP = "UserASP";
        private static readonly string stringDefault = string.Empty;
        private static readonly bool boolDefault = false;
        #endregion



        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault(username, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(username, value);
            }
        }


        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault(password, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(password, value);
            }
        }

        public static string TokenType
        {
            get
            {
                return AppSettings.GetValueOrDefault(tokenType, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(tokenType, value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(accessToken, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(accessToken, value);
            }
        }

        public static bool IsRemembered
        {
            get
            {
                return AppSettings.GetValueOrDefault(isRemembered, boolDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(isRemembered, value);
            }
        }

        public static string UserASP
        {
            get
            {
                return AppSettings.GetValueOrDefault(userASP, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userASP, value);
            }
        }
    }
}
