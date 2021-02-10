using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = HackChallenge.DAL.Entities.File;
using User = HackChallenge.DAL.Entities.User;

namespace HackChallenge.BLL.Commands
{
    public class BackCDCommand : IBackCDCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "cd..";

        public BackCDCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);
            if (user != null && user.isAuthorized)
            {
                var searchedDir = user.LinuxSystem.CurrentDirectory.Name.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                searchedDir.Remove(searchedDir.Last());
                if(searchedDir.Count == 0)
                {
                    await client.SendTextMessageAsync(chatId, "<code>Вы в корне</code>", ParseMode.Html);
                    return true;
                }

                LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                PreviousDirectory previousDirectory = await _unitOfWork.PreviousDirectoryRepository.GetByIdAsync(linuxSystem.PreviousDirectoryId);
                MainDirectory mainDirectory = await _unitOfWork.MainDirectoryRepository.GetByIdAsync(linuxSystem.MainDirectoryId);

                if (searchedDir.Last() == mainDirectory.Name)
                {
                    List<Directory> directories = GetOfMainDir(mainDirectory).dirs;
                    List<File> files = GetOfMainDir(mainDirectory).files;

                    user.LinuxSystem.PreviousDirectory = new PreviousDirectory()
                    {
                        Directories = directories,
                        Files = files,
                        Name = mainDirectory.Name,
                        TimeOfCreating = mainDirectory.TimeOfCreating
                    };

                    user.LinuxSystem.CurrentDirectory = new CurrentDirectory()
                    {
                        Directories = directories,
                        Files = files,
                        Name = mainDirectory.Name,
                        TimeOfCreating = mainDirectory.TimeOfCreating
                    };

                    await _unitOfWork.SaveAsync();
                    await client.SendTextMessageAsync(chatId, "<code>Папка изменена</code>", ParseMode.Html);
                    return true;
                }

                List<Directory> dirsOfPrev = _unitOfWork.DirectoryRepository.GetDirsByPrevDirId(previousDirectory.Id).ToList();
                List<File> filesOfPrevDir = _unitOfWork.FileRepository.GetFilesByPrevDirId(previousDirectory.Id).ToList();

                user.LinuxSystem.CurrentDirectory = new CurrentDirectory()
                {
                    Directories = dirsOfPrev,
                    Files = filesOfPrevDir,
                    Name = previousDirectory.Name,
                    TimeOfCreating = previousDirectory.TimeOfCreating
                };

                await _unitOfWork.SaveAsync();

                Directory previousDir = null;
                searchedDir.Remove(searchedDir.Last());
                foreach (var dir in user.LinuxSystem.AllDirectories)
                {
                    previousDir = FindDir(dir, searchedDir.Last());
                }

                string path = "";
                int counter = 0;

                foreach(var dir in searchedDir)
                {
                    counter++;
                    if (counter == searchedDir.Count)
                    {
                        path += dir;
                    }

                    path += $"{dir}/";
                }

                if (previousDir != null)
                {
                    user.LinuxSystem.PreviousDirectory = new PreviousDirectory()
                    {
                        Directories = previousDir.Directories,
                        Files = previousDir.Files,
                        Name = path,
                        TimeOfCreating = previousDir.TimeOfCreating
                    };
                }
                else
                {
                    if(searchedDir.Last() == mainDirectory.Name)
                    {
                        List<Directory> mainDirs = GetOfMainDir(mainDirectory).dirs;
                        List<File> files = GetOfMainDir(mainDirectory).files;

                        user.LinuxSystem.PreviousDirectory = new PreviousDirectory()
                        {
                            Directories = mainDirs,
                            Files = files,
                            Name = mainDirectory.Name,
                            TimeOfCreating = mainDirectory.TimeOfCreating
                        };
                    }
                }

                await _unitOfWork.SaveAsync();
                await client.SendTextMessageAsync(chatId, "<code>Папка изменена</code>", ParseMode.Html);

                return true;
            }

            return false;
        }

        private (List<Directory> dirs, List<File> files) GetOfMainDir(MainDirectory mainDirectory)
        {
            List<Directory> directories = _unitOfWork.DirectoryRepository.GetDirsByMainDirId(mainDirectory.Id).ToList();
            List<File> files = _unitOfWork.FileRepository.GetFilesByMainDirId(mainDirectory.Id).ToList();

            return (directories, files);
        }

        private Directory FindDir(Directory directory, string dirName)
        {
            if(directory != null)
            {
                directory = _unitOfWork.DirectoryRepository.GetInDirectory(directory);

                if (dirName == directory.Name)
                {
                    return directory;
                }

                if (directory.Directories != null)
                {
                    foreach (var dir in directory.Directories)
                    {
                        if (dir.Name == dirName)
                        {
                            Directory fullDir = _unitOfWork.DirectoryRepository.GetInDirectory(dir);
                            return fullDir;
                        }

                        return FindDir(dir, dirName);
                    }
                }
            }

            return null;
        }

        public bool IsContains(Message message)
        {
            return message.Text == Name;
        }
    }
}
