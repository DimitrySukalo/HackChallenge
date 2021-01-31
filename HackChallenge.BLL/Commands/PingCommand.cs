using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HackChallenge.DAL.Entities.User;

namespace HackChallenge.BLL.Commands
{
    public class PingCommand : IPingCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        public string Name => "ping";

        public PingCommand(IUnitOfWork unitOfWork)
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
                if (message.Text == Name)
                {
                    await client.SendTextMessageAsync(chatId, "<code>Пожалуйста, введите ресурс!</code>", ParseMode.Html);
                    return false;
                }
                else
                {
                    string resourse = message.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    if (resourse.Contains(".com") || resourse.Contains(".ua") || resourse.Contains(".ru"))
                    {
                        if (user.LinuxSystem.IsConnectedTheInternet)
                        {
                            await client.SendTextMessageAsync(chatId, $"<code>Обмен пакетами с {resourse} с 32 байтами данных:</code>", ParseMode.Html);
                            for (int i = 0; i < 5; i++)
                            {
                                await client.SendTextMessageAsync(chatId, $"<code>Ответ от {resourse}: число байт=32 время={new Random().Next(10, 50)}мс TTL=119</code>", ParseMode.Html);
                            }

                            return true;
                        }
                        else
                        {
                            await client.SendTextMessageAsync(chatId, $"<code>При проверке связи не удалось обнаружить узел {resourse}.\nПроверьте имя узла и повторите попытку.</code>", ParseMode.Html);
                            return false;
                        }
                    }
                    else
                    {
                        await client.SendTextMessageAsync(chatId, "<code>Неизвестный ресурс.</code>", ParseMode.Html);
                        return false;
                    }
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
