using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = HackChallenge.DAL.Entities.User;
using File = HackChallenge.DAL.Entities.File;
using Telegram.Bot.Types.Enums;
using System.Text;

namespace HackChallenge.BLL.Commands
{
    public class StartCommand : IStartCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public StartCommand(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), " was null.");

            _unitOfWork = unitOfWork;
        }

        public string Name => "/start";

        public async Task<bool> Execute(Message message, TelegramBotClient client)
        {
            long chatId = message.Chat.Id;

            List<Directory> directories = new List<Directory>()
            {
                new Directory()
                {
                    Name = "bin",
                    Path = "@root/bin",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "Downloads",
                    Path = "@root/Downloads",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "var",
                    Path = "@root/var",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "backups",
                    Path = "@root/backups",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "Music",
                    Path = "@root/Music",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "Videos",
                    Path = "@root/Videos",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>(),
                    Directories = new List<Directory>(),
                },
                new Directory()
                {
                    Name = "Files",
                    Path = "@root/Files",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>()
                    {
                        new File()
                        {
                            Name = "passwords.txt",
                            Path = "@root/Files/passwords.txt",
                            TimeOfCreating = DateTime.UtcNow,
                            Size = new Random().Next(60,700),
                            Text = GetPasswordValues().passwords
                        }
                    }
                },
            };

            User user = new User()
            {
                ChatId = chatId,
                FirstName = message.From.FirstName,
                UserName = message.From.Username,
                LastName = message.From.LastName,
                HaveLinuxPermission = false,
                LinuxSystem = new LinuxSystem()
                {
                    IsConnectedTheInternet = false,
                    WifiModule = new WifiModule()
                    {
                        ModuleMode = ModuleMode.Managed,
                        Name = "wlan0",
                        Wifis = GetWifis()
                    },
                    AllDirectories = directories,
                    CurrentDirectory = new CurrentDirectory()
                    {
                        Directory = new Directory()
                        {
                            Directories = directories,
                            Files = new List<File>(),
                            TimeOfCreating = DateTime.UtcNow,
                            Path = "@root",
                            Name = "@root"
                        }
                    }
                },
                CountOfCrackWifi = 0
            };



            User tempUser = await _unitOfWork.UserAccessRepository.GetUserByChatId(chatId);
            if(tempUser == null)
            {
                await _unitOfWork.UserAccessRepository.AddAsync(user);
                await client.SendTextMessageAsync(chatId, "<code>Спасибо за регистрацию! Теперь мы можем начать ✅</code>", ParseMode.Html);
                await client.SendTextMessageAsync(chatId, "<code>Процесс подготовки...</code>", ParseMode.Html);
                await client.SendTextMessageAsync(chatId, "<code>Ещё немного...</code>", ParseMode.Html);
                await client.SendTextMessageAsync(chatId, "<code>Введите логин и пароль в формате login:password 🌐</code>", ParseMode.Html);

                return true;
            }

            await client.SendTextMessageAsync(chatId, "<code>Вы уже зарегестрированы! ✅</code>", ParseMode.Html);
            return false;
        }

        private (string passwords, string password) GetPasswordValues()
        {
            Dictionary<int, string> passwords = new Dictionary<int, string>()
            {
                { 1, "VV8STSXHGl7q07lv"},
                { 2, "LltXsi4dVZcpwYBU"},
                { 3, "oVXdAiOjtjpvWDPO"},
                { 4, "9HLfyMVWXhTXbqGo"},
                { 5, "07otvIQ5k9w6S4kk"},
                { 6, "NTc1hglHN82Qo7Rg"},
                { 7, "5f2v7kMKMeSgOZw5"},
                { 8, "J20NJhsvAYOgczJb"},
                { 9, "0nHvX9dVZxjF9LiQ"},
                { 10, "dD6scbRgGRdM11FK"}
            };

            string randomPass = passwords[new Random().Next(1, 10)];
            StringBuilder passwordsStr = new StringBuilder();
            foreach(var pass in passwords)
            {
                passwordsStr.Append($"{pass.Value}\n");
            }

            return (passwordsStr.ToString(), randomPass);
        }

        private ICollection<Wifi> GetWifis()
        {
            return new List<Wifi>()
            {
                new Wifi()
                {
                    BSSID = GetRandomBSSID(),
                    Name = "Home",
                    Password = GetPasswordValues().password,
                    QualityOfSignal = QualityOfSignal.Good,
                    Speed = new Random().Next(10, 100),
                    Channel = new Random().Next(1, 10),
                    Cipher = Cipher.CCMP,
                    EncryptionType = EncryptionType.WPA2
                },
                new Wifi()
                {
                    BSSID = GetRandomBSSID(),
                    Name = "Lan1979",
                    Password = GetPasswordValues().password,
                    QualityOfSignal = QualityOfSignal.Normal,
                    Speed = new Random().Next(10, 100),
                    Channel = new Random().Next(1, 10),
                    Cipher = Cipher.CCMP,
                    EncryptionType = EncryptionType.WEP
                },
                new Wifi()
                {
                    BSSID = GetRandomBSSID(),
                    Name = "TP_Link-003245",
                    Password = GetPasswordValues().password,
                    QualityOfSignal = QualityOfSignal.Bad,
                    Speed = new Random().Next(10, 100),
                    Channel = new Random().Next(1, 10),
                    Cipher = Cipher.TKIP,
                    EncryptionType = EncryptionType.WPA2
                },
                new Wifi()
                {
                    BSSID = GetRandomBSSID(),
                    Name = "Modem-13435",
                    Password = GetPasswordValues().password,
                    QualityOfSignal = QualityOfSignal.Good,
                    Speed = new Random().Next(10, 100),
                    Channel = new Random().Next(1, 10),
                    Cipher = Cipher.WEP,
                    EncryptionType = EncryptionType.WPA2
                }
            };
        }

        private string GetRandomBSSID()
        {
            Random rnd = new Random();
            return $"{rnd.Next(10,99)}:{rnd.Next(10, 99)}:{rnd.Next(0,9)}D:{rnd.Next(0, 9)}F:{rnd.Next(10, 99)}:{rnd.Next(0, 9)}E";
        }

        public bool IsContains(Message message)
        {
            return message.Text.Contains(Name);
        }
    }
}
