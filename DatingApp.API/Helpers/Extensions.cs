using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    //extend other classes in a helpful way
    public static class Extensions
    {
        //make application errors have correct headers
        public static void AddApplicationError(this HttpResponse response, string message)
        {
                response.Headers.Add("Application-Error", message);
                response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
                response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime theDateTime){
            var age =  DateTime.Today.Year - theDateTime.Year;
            //if they havent had their birthday this year remove 1 year so their age is correct
            if(theDateTime.AddYears(age) > DateTime.Today){
                age--;
            }
            return age;
        }
    }
}