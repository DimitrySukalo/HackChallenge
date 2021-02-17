using HackChallenge.BLL.Models;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace HackChallenge.Controllers
{
    public class BotController
    {
        private readonly TelegramBotClient _client;
        private readonly Bot _bot;

        public BotController(TelegramBotClient client,
                             Bot bot)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client), " was null.");
            _bot = bot ?? throw new ArgumentNullException(nameof(bot), " was null.");
        }

        public void StartBot()
        {
            _client.StartReceiving();
            _client.OnMessage += ClientOnMessage;

            Console.WriteLine("Bot server is started...");
        }

        private async void ClientOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Text)
            {
                var message = e.Message;
                var commands = _bot.Commands;
                bool result = false;

                foreach (var command in commands)
                {
                    if (command.IsContains(message))
                    {
                        result = await command.Execute(message, _client);
                    }
                }

                if (!result)
                    await _client.SendTextMessageAsync(e.Message.Chat.Id, "<code>Неизвестная команда</code>", ParseMode.Html);
            }
        }
    }
}
