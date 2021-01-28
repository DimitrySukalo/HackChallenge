using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = HackChallenge.DAL.Entities.User;
using File = HackChallenge.DAL.Entities.File;
using Telegram.Bot.Types.Enums;

namespace HackChallenge.BLL.Commands
{
    public class StartCommand : IStartCommand
    {
        private readonly ApplicationContext _db;
        private readonly IUserAccessRepository _userAccessRepository;

        public StartCommand(ApplicationContext context,
                            IUserAccessRepository userRepository)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context), " was null.");

            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository), " was null.");

            _db = context;
            _userAccessRepository = userRepository;
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
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>()
                },
                new Directory()
                {
                    Name = "Downloads",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>()
                },
                new Directory()
                {
                    Name = "var",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>()
                },
                new Directory()
                {
                    Name = "root",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>()
                },
                new Directory()
                {
                    Name = "backups",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>()
                },
                new Directory()
                {
                    Name = "Music",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>()
                },
                new Directory()
                {
                    Name = "Videos",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>()
                },
                new Directory()
                {
                    Name = "Files",
                    TimeOfCreating = DateTime.UtcNow,
                    Files = new List<File>()
                },
            };

            Modem modem = new Modem()
            {
                IPAddress = GetRandomIp(),
                Login = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),
                Wifis = new List<Wifi>()
                {
                    new Wifi()
                    {
                        BSSID = GetRandomBSSID(),
                        Name = "Home",
                        Password = Guid.NewGuid().ToString(),
                        QualityOfSignal = QualityOfSignal.Good,
                        Speed = new Random().Next(10, 100)
                    },
                    new Wifi()
                    {
                        BSSID = GetRandomBSSID(),
                        Name = "Lan1979",
                        Password = Guid.NewGuid().ToString(),
                        QualityOfSignal = QualityOfSignal.Normal,
                        Speed = new Random().Next(10, 100)
                    },
                    new Wifi()
                    {
                        BSSID = GetRandomBSSID(),
                        Name = "TP_Link-003245",
                        Password = Guid.NewGuid().ToString(),
                        QualityOfSignal = QualityOfSignal.Bad,
                        Speed = new Random().Next(10, 100)
                    },
                    new Wifi()
                    {
                        BSSID = GetRandomBSSID(),
                        Name = "Modem-13435",
                        Password = Guid.NewGuid().ToString(),
                        QualityOfSignal = QualityOfSignal.Good,
                        Speed = new Random().Next(10, 100)
                    }
                }
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
                    Directories = directories,
                    Modem = modem,
                    IsConnectedTheInternet = false
                }
            };



            bool isExist = _db.Users.Any(u => u.ChatId == chatId);
            if(!isExist)
            {
                await _userAccessRepository.AddAsync(user);
                await client.SendTextMessageAsync(chatId, "<code>Спасибо за регистрацию! Теперь мы можем начать ✅</code>", ParseMode.Html);
                await client.SendTextMessageAsync(chatId, "<code>Процесс подготовки...</code>", ParseMode.Html);
                await client.SendTextMessageAsync(chatId, "<code>Ещё немного...</code>", ParseMode.Html);
                await client.SendTextMessageAsync(chatId, "<code>Введите логин и пароль в формате login:password 🌐</code>", ParseMode.Html);

                return true;
            }

            await client.SendTextMessageAsync(chatId, "<code>Вы уже зарегестрированы! ✅</code>", ParseMode.Html);
            return false;
        }

        private string GetRandomIp()
        {
            Random rnd = new Random();
            return $"{rnd.Next(0, 255)}.{rnd.Next(0, 255)}.{rnd.Next(0, 255)}.{rnd.Next(0, 255)}";
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
