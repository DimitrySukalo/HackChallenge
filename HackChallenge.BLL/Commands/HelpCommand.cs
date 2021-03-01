using HackChallenge.BLL.CommandDIInterfaces;
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
    public class HelpCommand : IHelpCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Name => "/help";


        public HelpCommand(IUnitOfWork unitOfWork)
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
                StringBuilder helpInfo = new StringBuilder();
                helpInfo.Append("<code>У нас доступны такие команды:\n");
                helpInfo.Append("   ping сайт - позволяеть пропинговать сайт\n");
                helpInfo.Append("   ls - показывает содержимое директории\n");
                helpInfo.Append("   ls -la - показывает детальное содержимое директории\n");
                helpInfo.Append("   pwd - показывает текущий путь\n");
                helpInfo.Append("   aireplay-ng - посылает пакеты на выбраную сеть\n");
                helpInfo.Append("   nmap ip - сканирует порты айпи адреса\n");
                helpInfo.Append("   netdiscover - показывает устройства которые вместе с вами в одной сети\n");
                helpInfo.Append("   ifconfig - можно посмотреть ваш ip адрес\n");
                helpInfo.Append("   airmon-ng start wlan0 - переводит wifi-модуль в MonitorMode\n");
                helpInfo.Append("   cd папка - можно перейти в выбраную папку\n");
                helpInfo.Append("   cd.. - возвращаеться на директорию назад\n");
                helpInfo.Append("   airodump-ng wlan0mon - выводит список wifi-сетей\n");
                helpInfo.Append("   aircrack-ng - подбирает пароль в wifi-сети\n");
                helpInfo.Append("   smtp-check ip- выводит список пользователей которые есть по выбраному ip-адресу</code>");

                await client.SendTextMessageAsync(chatId, helpInfo.ToString(), ParseMode.Html);
                return true;
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
