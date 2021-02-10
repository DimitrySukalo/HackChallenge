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
    public class PWDCommand : IPWDCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "pwd";

        public PWDCommand(IUnitOfWork unitOfWork)
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
                LinuxSystem linuxSystem = await _unitOfWork.LinuxRepository.GetByIdAsync(user.Id);
                if (linuxSystem != null)
                {
                    CurrentDirectory currentDirectory = await _unitOfWork.CurrentDirectoryRepository.GetByIdAsync(linuxSystem.CurrentDirId);
                    if (currentDirectory != null)
                    {
                        string pathName = currentDirectory.Name;
                        await client.SendTextMessageAsync(chatId, $"<code>{pathName}</code>", ParseMode.Html);

                        return true;
                    }

                    return false;
                }

                return false;
            }

            return false;
        }

        public bool IsContains(Message message)
        {
            return message.Text == Name;
        }
    }
}
