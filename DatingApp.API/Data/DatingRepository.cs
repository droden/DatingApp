using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using MVCDatingApp.API.Data;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public virtual bool AutoDetectChangesEnabled { get; set; }
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
           _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
          return await _context.Users.Include( p => p.Photos).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include( p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            //if the user hits save with no changes save anyways otherwise 
            //errors will occur because the framework will return false which is interpreted as an error condition
            
            AutoDetectChangesEnabled = false;
            return  await _context.SaveChangesAsync() > 0;
        }
    }
}