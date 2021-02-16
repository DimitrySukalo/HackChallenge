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
    public class DirectoryRepository : IDirectoryRepository
    {
        ApplicationContext _db;

        public DirectoryRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        public async Task<Directory> AddAsync(Directory entity)
        {
            if (entity != null)
            {
                await _db.Directories.AddAsync(entity);
                await _db.SaveChangesAsync();

                return entity;
            }

            return null;
        }

        public IEnumerable<Directory> GetAll()
        {
            IEnumerable<Directory> directories = _db.Directories;
            return directories;
        }

        public async Task<Directory> GetByIdAsync(int id)
        {
            Directory directory = await _db.Directories.Include(d => d.Directories)
                                                        .Include(d => d.Files).FirstOrDefaultAsync(d => d.Id == id);

            return directory;
        }

        public async Task<Directory> GetByPath(string path)
        {
            return await _db.Directories.FirstOrDefaultAsync(d => d.Path == path);
        }

        public IEnumerable<Directory> GetDirectoriesOfCurrentDirectory(int id)
        {
            IEnumerable<Directory> directories = _db.Directories.Where(d => d.CurrentDirectory.Id == id);
            return directories;
        }

        public async Task<IEnumerable<Directory>> GetDirsOfLinuxSystemId(int id)
        {
            LinuxSystem linuxSystem = await _db.LinuxSystems.Include(s => s.AllDirectories).FirstOrDefaultAsync(s => s.Id == id);
            IEnumerable<Directory> directories = linuxSystem.AllDirectories;
            return directories;
        }

        public Directory GetInDirectory(Directory directory)
        {
            Directory dir = _db.Directories.Include(d => d.Directories).Include(d => d.Files).FirstOrDefault(d => d.Id == directory.Id);
            return dir;
        }

        public Directory Remove(Directory entity)
        {
            if (entity != null)
            {
                _db.Directories.Remove(entity);
                _db.SaveChanges();

                return entity;
            }

            return null;
        }

        public async Task<Directory> RemoveByIdAsync(int id)
        {
            Directory directory = await _db.Directories.FirstOrDefaultAsync(d => d.Id == id);
            if (directory != null)
            {
                _db.Directories.Remove(directory);
                await _db.SaveChangesAsync();

                return directory;
            }

            return null;
        }
    }
}
