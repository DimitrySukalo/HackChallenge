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

                    LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                    if (linuxSystem != null)
                    {
                        CurrentDirectory currentDirectory = await _unitOfWork.CurrentDirectoryRepository.GetByIdAsync(linuxSystem.CurrentDirId);
                        if (currentDirectory != null)
                        {
                            Directory directory = _unitOfWork.DirectoryRepository.GetDirectoriesOfCurrentDirectory(currentDirectory.Id)
                                                                                 .FirstOrDefault(d => d.Name == text);

                            if (directory != null)
                            {
                                string currentDirName = currentDirectory.Name;

                                List<Directory> directories = _unitOfWork.DirectoryRepository.GetDirectoriesOfCurrentDirectory(currentDirectory.Id).ToList();
                                List<File> files = _unitOfWork.FileRepository.GetFilesByCurrentDirId(currentDirectory.Id).ToList();

                                PreviousDirectory previousDirectory = new PreviousDirectory()
                                {
                                    Name = currentDirName,
                                    TimeOfCreating = currentDirectory.TimeOfCreating
                                };

                                foreach(var dir in directories)
                                {
                                    dir.PreviousDirectory = previousDirectory;
                                }

                                foreach(var file in files)
                                {
                                    file.PreviousDirectory = previousDirectory;
                                }

                                user.LinuxSystem.PreviousDirectory = previousDirectory;

                                directory = _unitOfWork.DirectoryRepository.GetInDirectory(directory);

                                CurrentDirectory currentDir = new CurrentDirectory()
                                {
                                    Name = $"{currentDirName}/{directory.Name}",
                                    TimeOfCreating = directory.TimeOfCreating
                                };

                                foreach(var dir in directory.Directories)
                                {
                                    dir.CurrentDirectory = currentDir;
                                }

                                foreach(var file in directory.Files)
                                {
                                    file.CurrentDirectory = currentDir;
                                }

                                user.LinuxSystem.CurrentDirectory = currentDir;

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

                        return false;
                    }

                    return false;
                }
            }

            return false;
        }

        public bool IsContains(Message message)
        {
            return message.Text.Contains(Name);
        }
    }
}
