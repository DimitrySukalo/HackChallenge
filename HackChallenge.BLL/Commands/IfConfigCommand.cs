using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;

namespace HackChallenge.BLL.Commands
{
    public class IfConfigCommand : IIfConfingCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        public string Name => "ifconfig";

        public IfConfigCommand(IUnitOfWork unitOfWork)
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
                LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                if (linuxSystem != null)
                {
                    WifiModule wifiModule = await _unitOfWork.WifiModuleRepository.GetByIdAsync(linuxSystem.WifiModuleId);

                    if (wifiModule != null)
                    {
                        StringBuilder lanDetails = new StringBuilder();
                        lanDetails.Append("<code>lo: flags=73 UP,LOOPBACK,RUNNING  mtu 1500\n" +
                                          "inet 127.0.0.1  netmask 255.0.0.0\n" +
                                          "inet6::1  prefixlen 128  scopeid 0xfe  compat, link, site, host \n" +
                                          "loop(Local Loopback) </code>");

                        await client.SendTextMessageAsync(chatId, lanDetails.ToString(), ParseMode.Html);
                        lanDetails.Clear();

                        lanDetails.Append($"<code>{wifiModule.Name}: flags=4163 UP,BROADCAST,RUNNING,MULTICAST  mtu 1500\n" +
                                          "inet 192.168.1.104  netmask 255.255.255.0  broadcast 192.168.1.255\n" +
                                          "inet6 fe80::bd6d:680a: 8ca8: c15f  prefixlen 64  scopeid 0xfd  compat, link, site, host \n" +
                                          "ether f8: a2:d6: c8:f7: 4f(Ethernet)\n" +
                                          $"Mode: {GetWifiMode(wifiModule.ModuleMode)}</code>");

                        await client.SendTextMessageAsync(chatId, lanDetails.ToString(), ParseMode.Html);
                        return true;
                    }

                    return false;
                }

                return false;

            }

            return false;
        }

        private string GetWifiMode(ModuleMode moduleMode)
        {
            if (moduleMode == ModuleMode.Managed)
                return "Managed";

            return "Monitor";
        }

        public bool IsContains(Message message)
        {
            if (!string.IsNullOrWhiteSpace(message.Text))
                return message.Text.Contains(Name);

            return false;
        }
    }
}
