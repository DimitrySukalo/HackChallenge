using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;

namespace HackChallenge.BLL.Commands
{
    public class SendPackagesCommand : ISendPackagesCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "aireplay-ng";

        public SendPackagesCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);

            if(user != null && user.isAuthorized && user.LinuxSystem.WifiModule.ModuleMode == ModuleMode.Monitor)
            {
                string[] commandParams = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                bool isExist = user.LinuxSystem.WifiModule.Wifis.Any(w => w.BSSID == commandParams[3]);

                if(commandParams[1] == "--deauth" && commandParams[2] == "-a" &&
                   isExist && commandParams[4] == user.LinuxSystem.WifiModule.Name)
                {
                    for(int i = 0; i < 8; i++)
                    {
                        await client.SendTextMessageAsync(chatId, $"<code>{DateTime.UtcNow.ToString("HH:mm:ss")}  Sending DeAuth to broadcast -- BSSID: [{commandParams[3]}]</code>", ParseMode.Html);
                        Thread.Sleep(1000);
                    }

                    return true;
                }
                else
                {
                    await client.SendTextMessageAsync(chatId, "<code>Неверные данные</code>", ParseMode.Html);
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
