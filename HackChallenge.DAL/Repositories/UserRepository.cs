using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddRange(List<User> users)
        {
            if(users != null)
            {
                await _db.Users.AddRangeAsync(users);
                await _db.SaveChangesAsync();
            }
        }

        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> users = _db.Users;
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> GetByLinuxSystemIP(string ip)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.LinuxSystem.IP == ip);
        }

        public async Task<User> GetUserByChatId(long chatId)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.ChatId == chatId);
            return user;
        }

        public async Task<List<User>> GetUsersByGlobalNetworkId(int id)
        {
            return await _db.Users.Where(u => u.GlobalNetworkId == id).ToListAsync();
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
    }
}
