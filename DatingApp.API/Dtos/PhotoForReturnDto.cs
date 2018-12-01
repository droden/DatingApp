using System;

namespace DatingApp.API.Dtos
{
    public class PhotoForReturnDto
    {
        
            public int Id { get; set; }
            public string Url { get; set; }
            public string  Description { get; set; }
            public DateTime DateAdded { get; set; }
            public bool IsMainPhoto { get; set; }

            //property retured from cloudinary for the image
            public string PublicId { get; set; }

    }
}