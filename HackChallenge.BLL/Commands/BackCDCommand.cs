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
                LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                CurrentDirectory current = await _unitOfWork.CurrentDirectoryRepository.GetByIdAsync(linuxSystem.CurrentDirectoryId);
                Directory directory = _unitOfWork.DirectoryRepository.GetInDirectory(await _unitOfWork.DirectoryRepository.GetByIdAsync(current.DirectoryId));

                var searchedDir = directory.Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                searchedDir.Remove(searchedDir.Last());
                if(searchedDir.Count == 0)
                {
                    await client.SendTextMessageAsync(chatId, "<code>Вы в корне</code>", ParseMode.Html);
                    return true;
                }

                string dirPath = "";
                foreach(var path in searchedDir)
                {
                    dirPath += path;
                }

                Directory searchedDirectory = await _unitOfWork.DirectoryRepository.GetByPath(dirPath);
                if(searchedDirectory != null)
                {
                    user.LinuxSystem.CurrentDirectory.Directory = searchedDirectory;

                    await _unitOfWork.SaveAsync();
                    await client.SendTextMessageAsync(chatId, "<code>Папка изменена</code>", ParseMode.Html);

                    return true;
                }

                return false;
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
