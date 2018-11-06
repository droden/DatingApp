using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using MVCDatingApp.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MVCDatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.UserName == username);
            if(user == null) return null;

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)){
                return null;
            }   
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        { 
            bool passwordsAreEqual;

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
           {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                passwordsAreEqual =  System.Linq.Enumerable.SequenceEqual(computedHash, passwordHash);
                //the following should work but doesnt. 1+1 != 2
                // passwordsAreEqual = computedHash.Equals(passwordHash);
           }
            return passwordsAreEqual;
        }

        public async Task<User> Register(User user, string password)
        {
             byte[] passwordHash, passwordSalt;
             //set hash and salt
             CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            //create the user
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
           using (var hmac = new System.Security.Cryptography.HMACSHA512())
           {
               passwordSalt = hmac.Key;
               passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

           }

        }

        public async Task<bool> UserExists(string username)
        {
            //this is probably bad because people can verify user names maliciously
            //but im following the demo for now
             if(await _context.Users.AnyAsync(x=>x.UserName == username))
             return true;

             return false;
        }
    }
}