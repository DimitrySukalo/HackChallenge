using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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

                if (searchedDir.Last() == user.LinuxSystem.MainDirectory.Name)
                {
                    user.LinuxSystem.PreviousDirectory = new PreviousDirectory()
                    {
                        Directories = user.LinuxSystem.MainDirectory.Directories,
                        Files = user.LinuxSystem.MainDirectory.Files,
                        Name = user.LinuxSystem.MainDirectory.Name,
                        TimeOfCreating = user.LinuxSystem.MainDirectory.TimeOfCreating
                    };

                    user.LinuxSystem.CurrentDirectory = new CurrentDirectory()
                    {
                        Directories = user.LinuxSystem.MainDirectory.Directories,
                        Files = user.LinuxSystem.MainDirectory.Files,
                        Name = user.LinuxSystem.MainDirectory.Name,
                        TimeOfCreating = user.LinuxSystem.MainDirectory.TimeOfCreating
                    };

                    await _unitOfWork.SaveAsync();
                    await client.SendTextMessageAsync(chatId, "<code>Папка изменена</code>", ParseMode.Html);
                    return true;
                }


                user.LinuxSystem.CurrentDirectory = new CurrentDirectory()
                {
                    Directories = user.LinuxSystem.PreviousDirectory.Directories,
                    Files = user.LinuxSystem.PreviousDirectory.Files,
                    Name = user.LinuxSystem.PreviousDirectory.Name,
                    TimeOfCreating = user.LinuxSystem.PreviousDirectory.TimeOfCreating
                };

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
                    if(searchedDir.Last() == user.LinuxSystem.MainDirectory.Name)
                    {
                        user.LinuxSystem.PreviousDirectory = new PreviousDirectory()
                        {
                            Directories = user.LinuxSystem.MainDirectory.Directories,
                            Files = user.LinuxSystem.MainDirectory.Files,
                            Name = user.LinuxSystem.MainDirectory.Name,
                            TimeOfCreating = user.LinuxSystem.MainDirectory.TimeOfCreating
                        };
                    }
                }

                await _unitOfWork.SaveAsync();
                await client.SendTextMessageAsync(chatId, "<code>Папка изменена</code>", ParseMode.Html);

                return true;
            }

            return false;
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
