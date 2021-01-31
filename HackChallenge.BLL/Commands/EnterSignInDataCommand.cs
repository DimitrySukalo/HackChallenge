using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Interfaces;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using User = HackChallenge.DAL.Entities.User;

namespace HackChallenge.BLL.Commands
{
    public class EnterSignInDataCommand : IEnterSignInDataCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "login:password";

        public EnterSignInDataCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);

            if(user != null)
            {
                string[] signInData = message.Text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if(signInData[0] == "Uzdzkip" && signInData[1] == "Bw&+2u" && !user.isAuthorized)
                {

                    user.CountOfCorrectUserName = 2;
                    user.CountOfIncorrectLoginData = 2;
                    user.isAuthorized = true;
                    _unitOfWork.ApplicationContext.Users.Update(user);
                    await _unitOfWork.SaveAsync();

                    await client.SendTextMessageAsync(chatId, "<code>Поздравляем, с успешным входом в систему!\n Вы не так просты как нам казалось👨🏼‍💻! \nЧто ж, я вижу вы знаете что такое шифрование и надеемся помните самый простой метод. Вот вам от нас сообщение \n" +
                                                              "0J/RgNC40LLQtdGC0YHRgtCy0YPQtdC8INCyINGB0LjRgdGC0LXQvNC1IExpbnV4ISDQndC+INGDINCy0LDRgSDQv9GA0L7QsdC70LXQvNGLINGBINC40L3RgtC10YDQvdC10YLQvtC8Li4u</code>",ParseMode.Html);
                    return true;
                }
                else if(signInData[0] == "Uzdzkip" && signInData[1] != "Bw&+2u" && !user.isAuthorized)
                {
                    user.CountOfCorrectUserName += 1;
                    await _unitOfWork.SaveAsync();

                    if (user.CountOfCorrectUserName == 1)
                    {
                        using (var stream = System.IO.File.Open("photo.jpg", FileMode.Open))
                        {
                            await client.SendPhotoAsync(chatId, new InputOnlineFile(stream), "<code>Ключ перед вами 🔑</code>", ParseMode.Html);
                        }
                    }

                    return false;
                }
                else
                {
                    user.CountOfIncorrectLoginData += 1;
                    _unitOfWork.ApplicationContext.Users.Update(user);
                    await _unitOfWork.SaveAsync();

                    if(user.CountOfIncorrectLoginData == 1)
                    {
                        await client.SendTextMessageAsync(chatId, "<code>Клавдий Цезарь может сказать тебе твоё имя, но ты должен вспомнить имя человека создавшего этот проект и сколько ему лет</code> 🔑", ParseMode.Html);
                    }

                    return false;
                }
            }

            return false;
        }

        public bool IsContains(Message message)
        {
            if (Regex.IsMatch(message.Text, "(.*?):(.*?)") && message.Text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Length == 2)
                return true;

            return false;
        }
    }
}
