using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Repositories
{
    public class UserRepository : IUserAccessRepository
    {
        ApplicationContext _db;

        public UserRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        public async Task<User> AddAsync(User entity)
        {
            if (entity != null)
            {
                await _db.Users.AddAsync(entity);
                await _db.SaveChangesAsync();

                return entity;
            }

            return null;
        }

        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> users = _db.Users;
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            User user = await _db.Users.Include(u=>u.LinuxSystem)
                                       .Include(u => u.LinuxSystem).ThenInclude(s => s.WifiModule).ThenInclude(m => m.Wifis)
                                       .Include(u => u.LinuxSystem)
                                       .ThenInclude(s => s.Directories)
                                       .ThenInclude(d => d.Files).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> GetUserByChatId(long chatId)
        {
            User user = await _db.Users.Include(u => u.LinuxSystem)
                                       .Include(u => u.LinuxSystem).ThenInclude(s => s.WifiModule).ThenInclude(m => m.Wifis)
                                       .Include(u => u.LinuxSystem)
                                       .ThenInclude(s => s.Directories)
                                       .ThenInclude(d => d.Files).FirstOrDefaultAsync(u => u.ChatId == chatId);
            return user;
        }

        public User Remove(User entity)
        {
            if (entity != null)
            {
                _db.Users.Remove(entity);
                _db.SaveChanges();

                return entity;
            }

            return null;
        }

        public async Task<User> RemoveByIdAsync(int id)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

                return user;
            }

            return null;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
