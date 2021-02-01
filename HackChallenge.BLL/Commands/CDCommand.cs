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
    public class CDCommand : ICDCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "cd";

        public CDCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);

            if(user != null && user.isAuthorized)
            {
                string[] parameters = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if(parameters[0] == "cd")
                {
                    var text = message.Text.Remove(0, 2).Trim();
                    if (!text.Contains("/"))
                    {
                        var directory = user.LinuxSystem.CurrentDirectory.Directories.FirstOrDefault(d => d.Name == text);

                        if (directory != null)
                        {
                            string currentDirName = user.LinuxSystem.CurrentDirectory.Name;
                            user.LinuxSystem.PreviousDirectory = new PreviousDirectory()
                            {
                                Directories = user.LinuxSystem.CurrentDirectory.Directories,
                                Name = user.LinuxSystem.CurrentDirectory.Name,
                                Files = user.LinuxSystem.CurrentDirectory.Files,
                                TimeOfCreating = user.LinuxSystem.CurrentDirectory.TimeOfCreating
                            };

                            user.LinuxSystem.CurrentDirectory = new CurrentDirectory()
                            {
                                Name = $"{currentDirName}/{directory.Name}",
                                Directories = directory.Directories,
                                Files = directory.Files,
                                TimeOfCreating = directory.TimeOfCreating
                            };

                            _unitOfWork.ApplicationContext.CurrentDirectories.Update(user.LinuxSystem.CurrentDirectory);
                            await _unitOfWork.SaveAsync();


                            await client.SendTextMessageAsync(chatId, "<code>Папка изменена</code>", ParseMode.Html);

                            return true;
                        }
                        else
                        {
                            await client.SendTextMessageAsync(chatId, "<code>Директории не найдено</code>", ParseMode.Html);
                            return false;
                        }
                    }
                    else
                    {
                        var dir = text.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                        Directory directory = null;
                        string path = parameters[1].ToString().Trim();

                        foreach(var linuxDir in user.LinuxSystem.CurrentDirectory.Directories)
                        {
                            directory = FindDir(linuxDir, dir);
                            if(directory != null)
                            {
                                break;
                            }
                        }

                        if(directory != null)
                        {
                            string currentDirName = user.LinuxSystem.CurrentDirectory.Name;

                            user.LinuxSystem.PreviousDirectory = new PreviousDirectory()
                            {
                                Directories = user.LinuxSystem.CurrentDirectory.Directories,
                                Name = user.LinuxSystem.CurrentDirectory.Name,
                                Files = user.LinuxSystem.CurrentDirectory.Files,
                                TimeOfCreating = user.LinuxSystem.CurrentDirectory.TimeOfCreating
                            };

                            user.LinuxSystem.CurrentDirectory = new CurrentDirectory()
                            {
                                Name = $"{currentDirName}/{path}",
                                Directories = directory.Directories,
                                Files = directory.Files,
                                TimeOfCreating = directory.TimeOfCreating
                            };

                            _unitOfWork.ApplicationContext.CurrentDirectories.Update(user.LinuxSystem.CurrentDirectory);
                            await _unitOfWork.SaveAsync();
                            await client.SendTextMessageAsync(chatId, "<code>Папка изменена</code>", ParseMode.Html);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //Найти предадущую дерикторию и установить в систему

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
            return message.Text.Contains(Name);
        }
    }
}
