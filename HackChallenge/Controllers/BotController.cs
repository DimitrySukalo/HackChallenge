using HackChallenge.BLL.Models;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;

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
            var message = e.Message;
            var commands = _bot.Commands;

            foreach (var command in commands)
            {
                if (command.IsContains(message))
                {
                    var result = await command.Execute(message, _client);
                    break;
                }
            }
        }
    }
}
