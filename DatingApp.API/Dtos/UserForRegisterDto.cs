using System; 
using System.ComponentModel.DataAnnotations; 

namespace MVCDatingApp.API.Dtos {

        public class UserForRegisterDto {
                [Required]
                [StringLength(64, MinimumLength = 2, ErrorMessage = "Username required and must be beteween 2 and 64 characters")]
                public string Username {get; set; }

                [Required]
                [StringLength(64, MinimumLength = 2, ErrorMessage = "Password required and must be beteween 2 and 64 characters")]
                public string Password {get; set; }
                [Required]
                public string Gender {get; set; }
                [Required]
                public string KnownAs {get; set; }
                [Required]
                public DateTime DateOfBirth {get; set; }
                [Required]
                public string City {get; set; }
                [Required]
                public string Country {get; set; }
         
                public DateTime Created {get; set; }
            
                public DateTime LastActive {get; set; }

                public UserForRegisterDto() {
                Created = LastActive = DateTime.Now; 
                }


        }
}