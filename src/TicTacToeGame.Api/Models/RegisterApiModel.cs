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
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [CustomPasswordFormat(ErrorMessage = "Passwords must be at least 6 characters and contains the following: upper case (A-Z), lower case (a-z), number (0-9) and a non alphanumeric special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
    }
}
