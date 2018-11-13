using System;
using System.Collections.Generic;
using DatingApp.API.Models;

namespace DatingApp.API.Dtos
{
    public class UserForDetailDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; } 
        public int Age { get; set; }
        public string  KnownAs { get; set; }
        public DateTime AccountCreated { get; set; }
        public DateTime LastActive { get; set; }
        public string IntroMessage { get; set; }
        public string LookingForMessage { get; set; }
        public string Interests {get;set;}
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoURL{get;set;}
        public ICollection<PhotosForDetailedDto> Photos { get; set; }
        
    }
}