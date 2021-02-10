using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;
using File = HackChallenge.DAL.Entities.File;
using HackChallenge.DAL.Entities;
using System.Linq;

namespace HackChallenge.BLL.Commands
{
    public class AirCrackNgCommand : IAirCrackNgCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "aircrack-ng";

        public AirCrackNgCommand(IUnitOfWork unitOfWork)
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
                string[] parameters = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if(parameters[0] == Name && parameters[2] == "-w")
                {
                    if (!parameters[1].Contains("/"))
                    {
                        string[] fileParams = parameters[1].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        File file = user.LinuxSystem.CurrentDirectory.Files.FirstOrDefault(f => f.Name == parameters[1]);
                        if (fileParams[1] == "cap" && file != null)
                        {

                        }
                        else
                        {
                            await client.SendTextMessageAsync(chatId, "<code>Файл не найден. Перейдите в папку с этим файлом.</code>", ParseMode.Html);
                            return false;
                        }
                    }
                    else
                    {
                        var filePath = parameters[1].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        filePath.Remove(filePath.LastOrDefault());

                    }
                }
                else
                {
                    await client.SendTextMessageAsync(chatId, "<code>Неверная команда</code>", ParseMode.Html);
                    return false;
                }
            }

            return false;
        }

        private Directory FindDir(Directory directory, string dirName)
        {
            if (directory != null)
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
