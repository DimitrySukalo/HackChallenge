using HackChallenge.BLL.CommandDIInterfaces;
using HackChallenge.DAL.DB;
using HackChallenge.DAL.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = HackChallenge.DAL.Entities.User;

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

            User user = new User()
            {
                ChatId = chatId,
                FirstName = message.From.FirstName,
                UserName = message.From.Username,
                LastName = message.From.LastName
            };

            bool isExist = _db.Users.Any(u => u.ChatId == chatId);
            if(!isExist)
            {
                await _userAccessRepository.AddAsync(user);
                await client.SendTextMessageAsync(chatId, "Спасибо за регистрацию! Теперь мы можем начать ✅");
                await client.SendTextMessageAsync(chatId, "Процесс подготовки...");
                await client.SendTextMessageAsync(chatId, "Ещё немного...");
                await client.SendTextMessageAsync(chatId, "Введите логин и пароль в формате login:password 🌐");

                return true;
            }

            await client.SendTextMessageAsync(chatId, "Вы уже зарегестрированы! ✅");
            return false;
        }

        public bool IsContains(Message message)
        {
            return message.Text.Contains(Name);
        }
    }
}
