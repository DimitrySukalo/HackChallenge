using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
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
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(message.Chat.Id);
            if(user != null && user.isAuthorized)
            {
                LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                if(linuxSystem != null && linuxSystem.IsConnectedTheInternet)
                {

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
