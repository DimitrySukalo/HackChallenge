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
        private readonly IUserAccessRepository _userAccessRepository;

        public string Name => "ls";

        public ShowDirsCommand(IUserAccessRepository userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository), " was null.");

            _userAccessRepository = userRepository;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            User user = await _userAccessRepository.GetUserByChatId(message.Chat.Id);
            if (user.isAuthorized)
            {
                if (message.Text == Name)
                {
                    StringBuilder dirs = new StringBuilder();

                    foreach (var dir in user.LinuxSystem.Directories)
                    {
                        dirs.Append($"<code>{dir.Name}</code>\n");
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
                        foreach (var dir in user.LinuxSystem.Directories)
                        {
                            dirs.Append($"<code>{dir.GetSizeOfDir()}\t{dir.TimeOfCreating.ToString("MMM", CultureInfo.InvariantCulture)}\t{dir.TimeOfCreating.Day}\t{dir.Name}</code>🗂\n");
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
