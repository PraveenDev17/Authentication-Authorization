using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;

namespace Assignment.DTO.Login
{
    public class ShowDataDto{
        [Key]
        public int Id { get; set; }
        
        public string UserName { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public string Credit_Card { get; set; }
        [AllowedValues ("Admin","Reader")]
        public string Role { get; set; }
    }
}