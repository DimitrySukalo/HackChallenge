using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;

namespace HackChallenge.BLL.Commands
{
    public class NetDiscoverCommand : INetDiscoverCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        public string Name => "netdiscover";

        public NetDiscoverCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(_unitOfWork), " was null.");
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(message.Chat.Id);
            if(user != null && user.isAuthorized)
            {
                LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                if(linuxSystem != null && linuxSystem.IsConnectedTheInternet)
                {
                    GlobalNetwork globalNetwork = await _unitOfWork.GlobalNetworkRepository.GetByIdAsync(user.GlobalNetworkId);
                    if(globalNetwork != null)
                    {
                        List<User> networkUsers = await _unitOfWork.UserAccessRepository.GetUsersByGlobalNetworkId(globalNetwork.Id);
                        if(networkUsers.Count > 0)
                        {
                            bool successed = networkUsers.Remove(user);
                            if (successed)
                            {
                                LinuxSystem victimSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(networkUsers.First().Id);
                                if(victimSystem != null)
                                {
                                    StringBuilder networkInfo = new StringBuilder();
                                    networkInfo.Append($"<code>IP            MAC             ESSID\n");
                                    networkInfo.Append($"{linuxSystem.IP}\t{linuxSystem.MACAddress}\t{user.UserName}\n");
                                    networkInfo.Append($"{victimSystem.IP}\t{victimSystem.MACAddress}\t{networkUsers.First().UserName}</code>");

                                    await client.SendTextMessageAsync(chatId, networkInfo.ToString(), ParseMode.Html);
                                    return true;
                                }
                            }
                        }
                    }

                    await client.SendTextMessageAsync(chatId, "<code>Вы не в сети!</code>", ParseMode.Html);
                    return false;
                }

                return false;
            }

            return false;
        }

        public bool IsContains(Message message)
        {
            if (!string.IsNullOrWhiteSpace(message.Text))
                return message.Text == Name;

            return false;
        }
    }
}
