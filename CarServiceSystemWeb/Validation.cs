using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace CarServiceSystemWeb
{
    public static class Validation
    {

        public static string Validate(string value, string PropertyName)
        {
            try
            {
                if (value.Equals(string.Empty))
                {
                    return ($"Empty Field");
                }
            }
            catch
            {
                return ($"Empty Field");
            }

            if (PropertyName == "Name" || PropertyName == "Surename")
            {
                return NameValidate(value.ToString(), PropertyName);
            }
            if (PropertyName == "IDCardNumber" || PropertyName == "EGN")
            {
                return IDandEGNValidate(value.ToString(), PropertyName);
            }
            if (PropertyName == "PhoneNumber")
            {
                return PhoneNumberValidate(value.ToString());
            }
            if (PropertyName == "Email")
            {
                return EmailValidate(value.ToString());
            }
            if (PropertyName == "VinNumber")
            {
                return VINValidate(value.ToString());
            }
            if (PropertyName == "RegNumber")
            {
                return RegNumberValidate(value.ToString());
            }
            if (PropertyName == "Price")
            {
                return PriceValidate(value.ToString());
            }
            if (PropertyName == "Brand" || PropertyName == "Model")
            {
                return Brand_ModelValidate(value.ToString(), PropertyName);
            }
            return null;
        }

        public static string NameValidate(string input, string propName)
        {
            // @" ^[a-zA-Z]+$"
            if (Regex.Match(input, @"^[A-Z]{1}[a-z]*$").Success)
                return null;
            else
            {
                return (" can contain only letters and must start with upper case!");
            }
        }

        public static string Brand_ModelValidate(string input, string propName)
        {
            // @" ^[a-zA-Z]+$"
            if (Regex.IsMatch(input, @"^[A-Z]{1}[a-zA-Z0-9]*$"))
                return null;
            else
            {
                return (propName+" name can contain only letters and numbers and must start with upper case!");
            }
        }

        public static string IDandEGNValidate(string input, string propName)
        {
            try
            {
                long testint;
                testint = long.Parse((String)input);
            }
            catch
            {
                return (propName + " must be 10 digit number!");
            }
            if (input.ElementAt(0) == '0')
            {
                return ( propName + " cannot start with 0!");
            }
            if (Regex.Match(input, @"^(\d{10})$").Success)
                return null;
            else
            {
                return(propName + " must be 10 digit number!");
            }
        }

        public static string PhoneNumberValidate(string input)
        {
            try
            {
                int testint;
                testint = Int32.Parse((String)input);
            }
            catch
            {
                return ( "Phone number must be 10 digit number!");
            }

            if (Regex.Match(input, @"^(\d{9})$").Success)
                return null;
            else
            {
                return  ( "Phone number must contain 9 digit!");
            }
        }

        public static string EmailValidate(string input)
        {
            try
            {
                MailAddress m = new MailAddress(input);
                return null;
            }
            catch (FormatException)
            {
                return ( "Wrong Email input!");
            }
        }

        public static string VINValidate(string input)
        {
            if (Regex.Match(input, @"^[A-Z0-9]{17}\z").Success)
                return null;
            else
            {
                return ( "VIN number must contain 17 digit!");
            }
        }

        public static string RegNumberValidate(string input)
        {

            if (Regex.Match(input, @"^[ABEKMНOPCTYX]{2}[0-9]{4}[ABEKMНOPCTYX]{2}\z").Success)
                return null;
            else
            {
                return ( "Registration number format is \"AB1234BA\" ");
            }
        }

        public static string PriceValidate(string input)
        {
            try
            {
                double testint;
                testint = double.Parse((String)input);
            }
            catch
            {
                return ( "Wrong input!");
            }
            return null;
        }

        public static bool IsEmpty(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return true;
        }
    }
}
