using System.ComponentModel.DataAnnotations;
using TicTacToeGame.Api.CustomValidation;

namespace TicTacToeGame.Api.Models
{
    public class RegisterApiModel
    {
        [Required]
        [CustomeEmailFormat(ErrorMessage = "Email must be in correct format.")]
        public string Email { get; set; }

        [Required]
        [CustomPasswordFormat(ErrorMessage = "Passwords must be at least 6 characters and max 100 that contains the following: upper case (A-Z), lower case (a-z), number (0-9) and a non alphanumeric special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
    }
}
