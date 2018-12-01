using DatingApp.API.Data; 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Options; 
using AutoMapper; 
using DatingApp.API.Helpers;
using CloudinaryDotNet;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using System.Security.Claims;
using CloudinaryDotNet.Actions;
using DatingApp.API.Models;
using System.Linq;

namespace DatingApp.API.Controllers {
[Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
        public class PhotosContoller:ControllerBase {
        private readonly IDatingRepository _repo; 
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosContoller(IDatingRepository repository, 
        IMapper mapper, 
        IOptions < CloudinarySettings > cloudinaryConfig)
        {
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            _repo = repository; 
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }



        [HttpGet("{id}", Name="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }
        

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto )
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if(file.Length > 0){
                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams(){
                                File = new FileDescription(file.Name, stream),
                                Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            //if none of the photos are the main photo this is their first photo so make it the main
            if(!userFromRepo.Photos.Any(u => u.IsMainPhoto))
            {
                photo.IsMainPhoto = true;
            }
           
            userFromRepo.Photos.Add(photo);
            if(await _repo.SaveAll()){

                //wait until sqlite creates the ID for thoe photo so we can set it
                 var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                //httpposts are supposed to return createdatroute 201 code
                return CreatedAtRoute("GetPhoto", new PhotoForReturnDto{ Id = photo.Id},photoToReturn ); 
            }
            
            return BadRequest("Could not add photo...");
        }
                [HttpPost("{photoId}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int photoId){

                    if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                      return Unauthorized();

                 var userFromRepo = await _repo.GetUser(userId);

                //check that the photoid being added belongs to the user
                 if(!userFromRepo.Photos.Any(p =>p.Id == photoId))
                 {
                     return Unauthorized();
                 }

                 var photoFromRepo = await _repo.GetPhoto(photoId);
                 if(photoFromRepo.IsMainPhoto)
                 return BadRequest("This is already the main photo");

                 var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);

                 currentMainPhoto.IsMainPhoto = false;
                 photoFromRepo.IsMainPhoto = true;

                if(await _repo.SaveAll())
                return NoContent();

                return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
                if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                      return Unauthorized();

                 var userFromRepo = await _repo.GetUser(userId);

                //check that the photoid being added belongs to the user
                 if(!userFromRepo.Photos.Any(p =>p.Id == id))
                 {
                     return Unauthorized();
                 }
                      var photoFromRepo = await _repo.GetPhoto(id);
                 if(photoFromRepo.IsMainPhoto)
                 
                 return BadRequest("Cant delete main photo");        


                if(photoFromRepo.PublicId != null){
                     var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                    var result = _cloudinary.Destroy(deleteParams); 
                    if(result.Result == "ok"){
                     _repo.Delete(photoFromRepo);
                 }            

                }
                else
                 _repo.Delete(photoFromRepo);
               
                 if(await _repo.SaveAll())   
                 return Ok();

                 return BadRequest("Unable to delete photo");

        }


}
}