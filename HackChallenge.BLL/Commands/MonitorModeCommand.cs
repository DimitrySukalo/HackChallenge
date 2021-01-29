using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;

namespace HackChallenge.BLL.Commands
{
    public class MonitorModeCommand : IMonitorModeCommand
    {
        private readonly IUserAccessRepository _userAccessRepository;

        public string Name => "airmon-ng start wlan0";

        public MonitorModeCommand(IUserAccessRepository userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository), " was null.");

            _userAccessRepository = userRepository;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _userAccessRepository.GetUserByChatId(message.Chat.Id);
            WifiModule module = user.LinuxSystem.WifiModule;

            if(user != null && user.isAuthorized && module.ModuleMode == ModuleMode.Managed)
            {
                module.ModuleMode = ModuleMode.Monitor;
                module.Name = "wlan0mon";
                await _userAccessRepository.SaveChangesAsync();

                await client.SendTextMessageAsync(chatId, "<code>Режим изменён</code>", ParseMode.Html);

                return true;
            }

            return false;
        }

        public bool IsContains(Message message)
        {
            return message.Text.Contains(Name);
        }
    }
}
