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
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "airmon-ng start wlan0";

        public MonitorModeCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);
            WifiModule module = await _unitOfWork.WifiModuleRepository.GetWifiModuleByLinuxSystemIdAsync(user.LinuxSystem.Id);

            if(user != null && user.isAuthorized && module.ModuleMode == ModuleMode.Managed)
            {
                module.ModuleMode = ModuleMode.Monitor;
                module.Name = "wlan0mon";
                _unitOfWork.ApplicationContext.WifiModules.Update(module);
                await _unitOfWork.SaveAsync();

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
