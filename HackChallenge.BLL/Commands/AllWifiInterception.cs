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
    public class AllWifiInterception : IAllWifiInterception
    {
        private readonly IUserAccessRepository _userAccessRepository;

        public string Name => "airodump-ng wlan0mon";

        public AllWifiInterception(IUserAccessRepository userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository), " was null.");

            _userAccessRepository = userRepository;
        }

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;
            User user = await _userAccessRepository.GetUserByChatId(chatId);
            if(user != null  && user.isAuthorized && user.LinuxSystem.WifiModule.ModuleMode == ModuleMode.Monitor)
            {
                StringBuilder wifis = new StringBuilder();
                foreach(var wifi in user.LinuxSystem.WifiModule.Wifis)
                {
                    wifis.Append($"BSSID: {wifi.BSSID}    SPEED: {wifi.Speed}мб/с    CH: {wifi.Channel}    CIP: {GetCipher(wifi.Cipher)}    ENC: {GetEncryption(wifi.EncryptionType)}    ESSID: {wifi.Name}\n");
                }

                await client.SendTextMessageAsync(chatId ,$"<code>{wifis}</code>", ParseMode.Html);

                return true;
            }

            return false;
        }

        private string GetCipher(Cipher cipher)
        {
            string cipherString = "";

            switch (cipher)
            {
                case Cipher.CCMP:
                    cipherString = "CCMP";
                    break;

                case Cipher.TKIP:
                    cipherString = "TKIP";
                    break;

                case Cipher.WEP:
                    cipherString = "WEP";
                    break;

                default:
                    cipherString = "UNKNOWN";
                    break;
            }

            return cipherString;
        }

        private string GetEncryption(EncryptionType encryptionType)
        {
            string enc = "";

            switch(encryptionType)
            {
                case EncryptionType.OPN:
                    enc = "OPN";
                    break;

                case EncryptionType.WEP:
                    enc = "WEP";
                    break;

                case EncryptionType.WPA2:
                    enc = "WPA2";
                    break;

                default:
                    enc = "UNKNOWN";
                    break;
            }

            return enc;
        }

        public bool IsContains(Message message)
        {
            return message.Text.Contains(Name);
        }
    }
}
