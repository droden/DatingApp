using System.ComponentModel.DataAnnotations;

namespace MVCDatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [StringLength(64, MinimumLength=4, ErrorMessage ="Username required and must be beteween 4 and 64 characters")]
        public string Username { get; set; }

        [Required]
        [StringLength(64, MinimumLength=4, ErrorMessage ="Password required and must be beteween 4 and 64 characters")]
        public string Password { get; set; }
    }
}