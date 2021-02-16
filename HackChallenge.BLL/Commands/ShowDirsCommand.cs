using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;
using File = HackChallenge.DAL.Entities.File;
using System.Linq;

namespace HackChallenge.BLL.Commands
{
    public class ShowDirsCommand : IShowDirsCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "ls";

        public ShowDirsCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(message.Chat.Id);
            if (user.isAuthorized)
            {
                StringBuilder dirs = new StringBuilder();
                LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                CurrentDirectory currentDirectory = await _unitOfWork.CurrentDirectoryRepository.GetByIdAsync(linuxSystem.CurrentDirectoryId);
                Directory directory = _unitOfWork.DirectoryRepository.GetInDirectory
                                                                    (await _unitOfWork.DirectoryRepository.GetByIdAsync(currentDirectory.DirectoryId));

                IEnumerable<Directory> directories = directory.Directories;
                IEnumerable<File> files = directory.Files;

                if (directories.Count() == 0 &&
                    files.Count() == 0)
                {
                    dirs.Append("<code>Папка пустая</code>");
                    await client.SendTextMessageAsync(message.Chat.Id, dirs.ToString(), ParseMode.Html);

                    return true;
                }
                if (message.Text == Name)
                {
                    foreach (var dir in directories)
                    {
                        dirs.Append($"<code>{dir.Name}</code>\n");
                    }

                    foreach (var file in files)
                    {
                        dirs.Append($"<code>{file.Name}</code>\n");
                    }

                    await client.SendTextMessageAsync(message.Chat.Id, dirs.ToString(), ParseMode.Html);
                    return true;
                }
                else
                {
                    string[] showDirsParams = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (showDirsParams[0] == Name && showDirsParams[1] == "-la")
                    {
                        List<int> ids = directories.Select(d => d.Id).ToList();
                        Dictionary<int, List<File>> dirFiles = _unitOfWork.FileRepository.GetFilesOfSomeDirs(ids);
                        foreach (var dir in directories)
                        {
                            dirs.Append($"<code>{dir.GetSizeOfDir(dirFiles[dir.Id])}\t{dir.TimeOfCreating.ToString("MMM", CultureInfo.InvariantCulture)}\t{dir.TimeOfCreating.Day}\t{dir.Name}</code>\n");
                        }
                        foreach(var file in files)
                        {
                            dirs.Append($"<code>{file.Size}\t{file.TimeOfCreating.ToString("MMM", CultureInfo.InvariantCulture)}\t{file.TimeOfCreating.Day}\t{file.Name}</code>\n");
                        }

                        await client.SendTextMessageAsync(message.Chat.Id, dirs.ToString(), ParseMode.Html);
                        return true;
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
