using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TicTacToeGame.Api.CustomValidation
{
    public class CustomPasswordFormat : ValidationAttribute
    {
        private string password { get; set; }
        public override bool IsValid(object value)
        {
            password = Convert.ToString(value);
            if (string.IsNullOrWhiteSpace(password))
                return false;

            try
            {
                return Regex.IsMatch(password,
                    @"^(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$",
                    RegexOptions.None, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
