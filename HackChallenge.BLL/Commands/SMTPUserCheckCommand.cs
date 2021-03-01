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
    public class SMTPUserCheckCommand : ISMTPUserCheckCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "smtp-check";

        public SMTPUserCheckCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);
            LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);

            if(user != null && user.isAuthorized && linuxSystem.IsConnectedTheInternet
                && linuxSystem != null)
            {
                string[] commandParams = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                User victimUser = await _unitOfWork.UserAccessRepository.GetByLinuxSystemIP(commandParams[1]);

                if(commandParams[0] == Name && victimUser != null)
                {
                    await client.SendTextMessageAsync(chatId, "<code>Поиск пользователей...</code>", ParseMode.Html);
                    await client.SendTextMessageAsync(chatId, $"<code>Login: {victimUser.UserName}</code>", ParseMode.Html);

                    return true;
                }

                await client.SendTextMessageAsync(chatId, "<code>Неверный ip адрес или название команды.</code>", ParseMode.Html);
                return false;
            }

            await client.SendTextMessageAsync(chatId, "<code>Произошла ошибка</code>", ParseMode.Html);
            return false;
        }

        public bool IsContains(Message message)
        {
            if (!string.IsNullOrWhiteSpace(message.Text))
                return message.Text.Contains(Name);

            return false;
        }
    }
}
