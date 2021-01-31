using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Interfaces;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;

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
                if (message.Text == Name)
                {
                    StringBuilder dirs = new StringBuilder();

                    foreach (var dir in user.LinuxSystem.CurrentDirectory.Directories)
                    {
                        dirs.Append($"<code>{dir.Name}</code>\n");
                    }

                    foreach (var file in user.LinuxSystem.CurrentDirectory.Files)
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
                        StringBuilder dirs = new StringBuilder();
                        foreach (var dir in user.LinuxSystem.CurrentDirectory.Directories)
                        {
                            dirs.Append($"<code>{dir.GetSizeOfDir()}\t{dir.TimeOfCreating.ToString("MMM", CultureInfo.InvariantCulture)}\t{dir.TimeOfCreating.Day}\t{dir.Name}</code>\n");
                        }
                        foreach(var file in user.LinuxSystem.CurrentDirectory.Files)
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
