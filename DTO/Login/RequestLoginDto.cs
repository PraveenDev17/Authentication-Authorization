using System.ComponentModel.DataAnnotations;

namespace Assignment.DTO.Login
{
    public class RequestLoginDto{
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}