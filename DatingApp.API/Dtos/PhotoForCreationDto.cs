using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Dtos
{
    //class to act as a go between from the database to the cloudinary file for the photo
    public class PhotoForCreationDto
    {
        //url created at cloudinary for file
        public string Url { get; set; }
        //file sent to cloudinary
        public IFormFile File { get; set; }
        public string Description { get; set; } 
        public DateTime DateAdded { get; set; }

        //id returned from cloudinary
        public string PublicId { get; set; }

        public PhotoForCreationDto ()
        {
            DateAdded = DateTime.Now;            
        }
        
    }
}