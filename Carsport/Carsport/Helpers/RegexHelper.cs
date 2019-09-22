namespace Carsport.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;

    public static class RegexHelper
    {

        public static List<string> validEmailExtension = new List<string>
        {
            "uca.es",
            "gmail.uca.es",
            "alum.uca.es",
            "uca.esu.es",
            "gm.uca.es",
        };

        public static bool IsValidEmailAddress(string emailaddress)
        {
            try
            {
                var email = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsUniversitaryEmailAddress(string emailaddress)
        {
            bool isValid = false;

            try
            {
                var emailExtension = emailaddress.Split('@');

                for (int i = 0; i < validEmailExtension.Count; i++)
                {
                    if (emailExtension[1] == validEmailExtension[i])
                    {
                        isValid = true;
                    }
                }

                return isValid;
            }
            catch (FormatException)
            {
                return isValid;
            }
        }

    }
}
